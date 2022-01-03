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
                if (!IsPlayerHost(player, lobby))
                {
                    lobby.Players.Add(player);
                }
                lobbies[lobby.Code] = lobby;
                playersInLobby.Add(player.UserName, LobbyManagerCallback);
                log.Info(string.Format(
                    "Player {0} joined lobby with code {1}", userName, lobbyCode)); 
            }

            UpdateLobbyOnClients(lobby);
        }

        public void ExitLobby(string userName, string lobbyCode)
        {
            if (lobbyCode!= null &&
                lobbies.ContainsKey(lobbyCode) &&
                playersConnected.ContainsKey(userName))
            {
                Player player = lobbies[lobbyCode].Players.FirstOrDefault(
                    auxiliarPlayer => auxiliarPlayer.UserName.Equals(userName));
                lobbies[lobbyCode].Players.Remove(player);
                playersInLobby.Remove(player.UserName);
                log.Info(string.Format(
                    "Player {0} exited from lobby {1}", player.Nickname, lobbyCode));
                if (lobbies[lobbyCode].Players.Count.Equals(0))
                {
                    lobbies.Remove(lobbyCode);
                    log.Info(string.Format(
                        "Lobby with code {0} deleted", lobbyCode));
                    UpdateLobbyOnClients(null);
                }
                else
                {
                    if (player.UserName.Equals(lobbies[lobbyCode].Host))
                    {
                        lobbies[lobbyCode].ChangeHost();
                        log.Info(string.Format(
                            "Host in Lobby {0} changed", lobbyCode));
                    }
                    UpdateLobbyOnClients(lobbies[lobbyCode]);
                }
            }
        }

        public void SendMessage(string lobbyCode, string message, string sender)
        {
            Message newMessage = new Message(message, sender);
            lobbies[lobbyCode].MessageHistory.Add(newMessage);
            log.Info(string.Format("Message sent in Lobby with code: {0}", lobbyCode));
            UpdateLobbyOnClients(lobbies[lobbyCode]);
        }

        public void SendInvite(Lobby lobby, string sender, string receiver)
        {
            throw new NotImplementedException();
        }

        #region LocalMethods
        private bool IsPlayerHost(Player player, Lobby lobby)
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
                    catch (CommunicationObjectAbortedException communicationAborted)
                    {
                        LogOut(playerInLobby.Key);
                        log.Error(communicationAborted.Message, communicationAborted);
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
