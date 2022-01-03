using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using BoggleService.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace BoggleService.Services
{

    public partial class BoggleServices : IGameManagerContract
    {
        private readonly Dictionary<string, Lobby> lobbies =
            new Dictionary<string, Lobby>();

        private const string Public = "Public";
        private const int lobbyCodeSize = 5;

        public void CreateLobby(LobbySettingsDTO lobbySettings)
        {
            if (playersConnected.ContainsKey(lobbySettings.CreatorUserName))
            {
                Player creator = null;
                creator = GameService.GetPlayer(lobbySettings.CreatorUserName, creator);
                string lobbyCode = GenerateLobbyCode();
                Lobby newLobby = new Lobby(
                    code: lobbyCode,
                    host: creator,
                    language: lobbySettings.Language,
                    privacy: lobbySettings.LobbyPrivacy,
                    gameMode: lobbySettings.GameMode,
                    roomSize: lobbySettings.NumberOfPlayers);

                try
                {
                    GameManagerCallback.GrantAccessToJoinLobby(newLobby);
                    lobbies.Add(newLobby.Code, newLobby);
                    log.Info(string.Format(
                        "Lobby with code {0} created", newLobby.Code));
                }
                catch (CommunicationObjectAbortedException)
                {
                    playersConnected.Remove(creator.UserName);
                }
            }
        }

        public void AskToJoinLobby(string userName, string lobbyCode)
        {
            if (playersConnected.ContainsKey(userName))
            {
                if (playersInLobby.Keys.Contains(userName))
                {
                    playersInLobby.Remove(userName);
                }

                Lobby lobby = lobbies.Values.Where(
                    lobbyAux => lobbyAux.Code.Equals(lobbyCode)).FirstOrDefault();
                if (lobby.Players.Count.Equals(lobby.Size) ||
                    lobby.Players.Count.Equals(0))
                {
                    lobbies.Remove(lobby.Code);
                    lobby = null;
                }
                GameManagerCallback.GrantAccessToJoinLobby(lobby); 
            }
        }

        public void SearchPublicLobbies()
        {
            var query = lobbies.Where(lobby => lobby.Value.Privacy.Equals(Public)).ToList();
            PublicLobbyPreviewDTO[] publicLobbiesDTOs = new PublicLobbyPreviewDTO[query.Count];

            int lobbyIndex = 0;
            foreach (var lobby in query)
            {
                if (lobby.Value.Players.Count > 0)
                {
                    publicLobbiesDTOs[lobbyIndex] = ManualMapper.CreatePublicLobbyPreviewDTO(lobby.Value);
                    lobbyIndex++; 
                }
            }

            GameManagerCallback.DisplayPublicLobbies(publicLobbiesDTOs);
        }

        public void UpdatePublicLobbies()
        {
            var query = lobbies.Where(lobby => lobby.Value.Privacy.Equals(Public)).ToList();
            PublicLobbyPreviewDTO[] publicLobbiesDTOs = new PublicLobbyPreviewDTO[query.Count];

            int lobbyIndex = 0;
            foreach (var lobby in query)
            {
                publicLobbiesDTOs[lobbyIndex] = ManualMapper.CreatePublicLobbyPreviewDTO(lobby.Value);
                lobbyIndex++;
            }

            GameManagerCallback.RefreshPublicLobbies(publicLobbiesDTOs);
        }

        #region Local Methods

        private string GenerateLobbyCode()
        {
            string lobbyCode;

            do
            {
                lobbyCode = new string(Enumerable
                    .Repeat(characters, lobbyCodeSize)
                    .Select(characters => characters[random.Next(characters.Length)])
                    .ToArray());
            } while (IsFriendCodeInUse(lobbyCode));

            return lobbyCode;
        } 
        #endregion

        IGameManagerCallback GameManagerCallback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IGameManagerCallback>();
            }
        }
    }
}
