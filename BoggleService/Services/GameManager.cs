using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace BoggleService.Services
{

    public partial class BoggleServices
    {
        private readonly Dictionary<string, Lobby> lobbies = new Dictionary<string, Lobby>();
        private const string Public = "Public";
        private const int lobbyCodeSize = 5;

        public void CreateLobby(LobbySettingsDTO lobbySettings)
        {
            Player creator = null;
            creator = GameService.GetPlayer(lobbySettings.CreatorUserName, creator);
            string lobbyCode = GenerateLobbyCode();
            Lobby newLobby = new Lobby(
                code: lobbyCode,
                creator: creator,
                language: lobbySettings.Language,
                privacy: lobbySettings.LobbyPrivacy,
                gameMode: lobbySettings.GameMode,
                roomSize: lobbySettings.NumberOfPlayers);

            lobbies.Add(newLobby.Code, newLobby);
            Callback.JoinLobby(newLobby);
        }

        public void SearchPublicLobbies()
        {
            var query = lobbies.Where(lobby => lobby.Value.Privacy.Equals(Public)).ToList();
            PublicLobbyPreviewDTO[] publicLobbiesDTOs = new PublicLobbyPreviewDTO[query.Count];

            int lobbyIndex = 0;
            foreach (var lobby in query)
            {
                publicLobbiesDTOs[lobbyIndex] = ManualMapper.CreatePublicLobbyPreviewDTO(lobby.Value);
                lobbyIndex++;
            }

            Callback.DisplayPublicLobbies(publicLobbiesDTOs);
        }

        public void UpdatePublicLobbies()
        {
            throw new NotImplementedException();
        }

        public void JoinLobbyByCode(string userName, string lobbyCode)
        {
            Lobby lobby = lobbies.Values.Where(lobbyAux => lobbyAux.Code.Equals(lobbyCode)).FirstOrDefault();
            Player player = new Player();
            lobby.Players.Add(GameService.GetPlayer(userName, player));
            Callback.JoinLobby(lobby);
        }

        private string GenerateLobbyCode()
        {
            string lobbyCode;
            do
            {
                lobbyCode = new string(Enumerable
                    .Repeat(characters, lobbyCodeSize)
                    .Select(characters => characters[random.Next(characters.Length)]).ToArray());
            } while (IsFriendCodeInUse(lobbyCode));

            return lobbyCode;
        }
    }
}
