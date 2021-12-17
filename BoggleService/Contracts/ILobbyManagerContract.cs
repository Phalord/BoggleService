using BoggleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoggleService.Contracts
{
    [ServiceContract(
       CallbackContract = typeof(ILobbyManagerCallback),
       SessionMode = SessionMode.Required)]
    public interface ILobbyManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void JoinLobby(string userName, string lobbyCode);

        [OperationContract(IsOneWay = true)]
        void ExitLobby(string userName, string lobbyCode);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Lobby lobby, string body, string sender);

        [OperationContract(IsOneWay = true)]
        void SendInvite(Lobby lobby, string sender, string receiver);

        [OperationContract(IsOneWay = true)]
        void ChangeMatchSettings(Lobby lobby);
    }

    [ServiceContract]
    public interface ILobbyManagerCallback
    {
        [OperationContract]
        void UpdateLobby(Lobby lobby);
    }
}
