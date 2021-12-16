using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using BoggleService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

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
            playersConnected[creator.UserName] = Callback;
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
            var query = lobbies.Where(lobby => lobby.Value.Privacy.Equals(Public)).ToList();
            PublicLobbyPreviewDTO[] publicLobbiesDTOs = new PublicLobbyPreviewDTO[query.Count];

            int lobbyIndex = 0;
            foreach (var lobby in query)
            {
                publicLobbiesDTOs[lobbyIndex] = ManualMapper.CreatePublicLobbyPreviewDTO(lobby.Value);
                lobbyIndex++;
            }

            Callback.RefreshPublicLobbies(publicLobbiesDTOs);
        }

        public void JoinLobbyByCode(string userName, string lobbyCode)
        {
            Lobby lobby = lobbies.Values.Where(lobbyAux => lobbyAux.Code.Equals(lobbyCode)).FirstOrDefault();
            Player player = GameService.GetPlayer(userName, new Player());
            lobby.Players.Add(player);
            playersConnected[player.UserName] = Callback;
            Callback.JoinLobby(lobby);
            UpdateLobbyOnClients(lobby, userName);
        }

        private void UpdateLobbyOnClients(Lobby lobby, string userName)
        {
            Dictionary<string, IBoggleServicesCallback> playersInLobby = GetPlayersInLobby(lobby);

            foreach (var playerInLobby in playersInLobby)
            {
                try
                {
                    playerInLobby.Value.UpdateLobby(lobby);
                }
                catch(CommunicationObjectAbortedException)
                {
                    lobby.Players.Remove(lobby.Players.FirstOrDefault(
                        player => player.UserName.Equals(playerInLobby.Key)));
                    playersConnected.Remove(playerInLobby.Key);
                }
            }
        }

        public void SendMessage(Lobby lobby, string message, string sender)
        {
            Dictionary<string, IBoggleServicesCallback> playersInLobby = GetPlayersInLobby(lobby);
            Message newMessage = new Message(message, sender);

            foreach (var playerInLobby in playersInLobby)
            {
                try
                {
                    playerInLobby.Value.DeliverMessage(newMessage);
                }
                catch (CommunicationObjectAbortedException)
                {
                    lobby.Players.Remove(lobby.Players.FirstOrDefault(
                        player => player.UserName.Equals(playerInLobby.Key)));
                    playersConnected.Remove(playerInLobby.Key);
                }
            }
        }

        private Dictionary<string, IBoggleServicesCallback> GetPlayersInLobby(
            Lobby lobby)
        {
            Dictionary<string, IBoggleServicesCallback> playersInLobby =
                   new Dictionary<string, IBoggleServicesCallback>();

            foreach (Player player in lobby.Players)
            {
                playersInLobby.Add(
                    player.UserName, playersConnected[player.UserName]);
            }

            return playersInLobby;
        }

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
    }
}
