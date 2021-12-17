using BoggleModel;
using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
    [ServiceContract(
       CallbackContract = typeof(IGameManagerCallback),
       SessionMode = SessionMode.Required)]
    public interface IGameManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void CreateLobby(LobbySettingsDTO lobbySettings);

        [OperationContract(IsOneWay = true)]
        void AskToJoinLobby(string userName, string lobbyCode);

        [OperationContract(IsOneWay = true)]
        void SearchPublicLobbies();

        [OperationContract(IsOneWay = true)]
        void UpdatePublicLobbies();
    }

    [ServiceContract]
    public interface IGameManagerCallback
    {
        [OperationContract]
        void GrantAccessToJoinLobby(Lobby lobby);

        [OperationContract]
        void DisplayPublicLobbies(PublicLobbyPreviewDTO[] publicLobbies);

        [OperationContract]
        void RefreshPublicLobbies(PublicLobbyPreviewDTO[] publicLobbies);
    }
}
