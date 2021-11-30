using BoggleModel;
using BoggleModel.DataAccess;
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
            Lobby newLobby = new Lobby(lobbyCode, creator, lobbySettings.Language);

            lobbies.Add(newLobby.Code, newLobby);
            Callback.JoinLobby(newLobby);
        }

        public void SearchPublicLobbies()
        {
            var query = lobbies.Where(
                lobby => lobby.Value.Privacy.Equals(Public)).ToList();
            List<Lobby> publicLobbies = new List<Lobby>();
            foreach (var publicLobby in query)
            {
                publicLobbies.Add(publicLobby.Value);
            }

            Callback.DisplayPublicLobbies(publicLobbies);
        }

        public void UpdatePublicLobbies()
        {
            throw new NotImplementedException();
        }

        public void JoinPrivateLobbie(string userName, string lobbyCode)
        {
            throw new NotImplementedException();
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
