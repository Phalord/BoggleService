using BoggleModel;
using System.ServiceModel;

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
        void SendMessage(string lobbyCode, string body, string sender);

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
