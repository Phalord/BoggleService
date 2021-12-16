using BoggleModel;
using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
    [ServiceContract(CallbackContract = typeof(IBoggleServicesCallback),
        SessionMode = SessionMode.Required)]
    public interface IBoggleServiceContracts
    {
        #region User Manager Service

        [OperationContract(IsOneWay = true)]
        void LogIn(string userName, string password);

        [OperationContract(IsOneWay = true)]
        void CreateAccount(AccountDTO accountDTO);

        [OperationContract(IsOneWay = true)]
        void ValidateEmail(string validationCode, string email);

        #endregion

        #region Game Manager Service

        [OperationContract(IsOneWay = true)]
        void CreateLobby(LobbySettingsDTO lobbySettings);

        [OperationContract(IsOneWay = true)]
        void SearchPublicLobbies();

        [OperationContract(IsOneWay = true)]
        void UpdatePublicLobbies();

        [OperationContract(IsOneWay = true)]
        void JoinLobbyByCode(string userName, string lobbyCode);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Lobby lobby, string body, string sender);

        #endregion
    }

    [ServiceContract]
    public interface IBoggleServicesCallback
    {
        [OperationContract]
        void GrantAccess(string accessStatus, AccountDTO accountInfoDTO);

        [OperationContract]
        void AskForEmailValidation(string accountCreationStatus, string userEmail);

        [OperationContract]
        void GrantValidation(string validationStatus, AccountDTO accountInfoDTO);

        [OperationContract]
        void JoinLobby(Lobby lobby);

        [OperationContract]
        void DisplayPublicLobbies(PublicLobbyPreviewDTO[] publicLobbies);

        [OperationContract]
        void RefreshPublicLobbies(PublicLobbyPreviewDTO[] publicLobbies);

        [OperationContract]
        void UpdateLobby(Lobby lobby);

        [OperationContract]
        void DeliverMessage(Message message);
    }
}
