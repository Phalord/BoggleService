using BoggleModel;
using BoggleModel.DataAccess;
using BoggleService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace BoggleService.Services
{
    public partial class BoggleServices : ILobbyManagerContract
    {
        private readonly Dictionary<string, ILobbyManagerCallback> playersInLobby =
            new Dictionary<string, ILobbyManagerCallback>();

        public void ChangeMatchSettings(Lobby lobby)
        {
            throw new NotImplementedException();
        }

        public void JoinLobby(string userName, string lobbyCode)
        {
            Lobby lobby = lobbies.Values.Where(
                lobbyAux => lobbyAux.Code.Equals(lobbyCode)).FirstOrDefault();

            if (!(lobby is null))
            {
                Player player = GameService.GetPlayer(userName, new Player());
                if (!PlayerIsCreator(player, lobby))
                {
                    lobby.Players.Add(player);
                }
                lobbies[lobby.Code] = lobby;
                playersInLobby.Add(player.UserName, LobbyManagerCallback);
            }

            UpdateLobbyOnClients(lobby);
        }

        public void ExitLobby(string userName, string lobbyCode)
        {
            Lobby lobby = lobbies.Values.Where(
                lobbyAux => lobbyAux.Code.Equals(lobbyCode)).FirstOrDefault();

            if (!(lobby is null))
            {
                Player player = lobby.Players.FirstOrDefault(
                    auxiliarPlayer => auxiliarPlayer.UserName.Equals(userName));
                lobby.Players.Remove(player);
                playersInLobby.Remove(player.UserName);
                if (player.UserName.Equals(lobby.Host))
                {
                    lobby.ChangeHost();
                }
                if (lobby.Players.Count.Equals(0))
                {
                    lobbies.Remove(lobby.Code);
                    lobby = null;
                }
                else
                {
                    lobbies[lobby.Code] = lobby;
                }
            }
            UpdateLobbyOnClients(lobby);
        }

        public void SendMessage(string lobbyCode, string message, string sender)
        {
            Message newMessage = new Message(message, sender);

            lobbies[lobbyCode].MessageHistory.Add(newMessage);
            UpdateLobbyOnClients(lobbies[lobbyCode]);
        }

        public void SendInvite(Lobby lobby, string sender, string receiver)
        {
            throw new NotImplementedException();
        }

        #region LocalMethods
        private bool PlayerIsCreator(Player player, Lobby lobby)
        {
            return lobby.Host.Equals(player.UserName);
        }

        private void UpdateLobbyOnClients(Lobby lobby)
        {
            if (!(lobby is null))
            {
                Dictionary<string, ILobbyManagerCallback> playersInLobby = GetPlayersInLobby(lobby);

                foreach (var playerInLobby in playersInLobby)
                {
                    try
                    {
                        playerInLobby.Value.UpdateLobby(lobby);
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        lobby.Players.Remove(lobby.Players.FirstOrDefault(
                            player => player.UserName.Equals(playerInLobby.Key)));
                        playersConnected.Remove(playerInLobby.Key);
                    }
                }
            }
        }

        private Dictionary<string, ILobbyManagerCallback> GetPlayersInLobby(Lobby lobby)
        {
            Dictionary<string, ILobbyManagerCallback> playersInLobbyAux =
                   new Dictionary<string, ILobbyManagerCallback>();

            foreach (Player player in lobby.Players)
            {
                playersInLobbyAux.Add(
                    player.UserName, playersInLobby[player.UserName]);
            }

            return playersInLobbyAux;
        }
        #endregion

        ILobbyManagerCallback LobbyManagerCallback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<ILobbyManagerCallback>();
            }
        }
    }
}
