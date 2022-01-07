using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
    /// <summary>
    /// Contracts related to user management as account
    /// creation, authentication or email validation.
    /// </summary>
    [ServiceContract(
        CallbackContract = typeof(IUserManagerCallback),
        SessionMode = SessionMode.Required)]
    public interface IUserManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void LogIn(string userName, string password);

        [OperationContract(IsOneWay = true)]
        void LogOut(string username);

        [OperationContract(IsOneWay = true)]
        void CreateAccount(AccountDTO accountDTO);

        [OperationContract(IsOneWay = true)]
        void ValidateEmail(string validationCode, string email);
    }

    /// <summary>
    /// Client callbacks related to the user manager service.
    /// </summary>
    [ServiceContract]
    public interface IUserManagerCallback
    {
        [OperationContract]
        void GrantAccess(string accessStatus, AccountDTO accountInfoDTO);

        [OperationContract]
        void CloseSession();

        [OperationContract]
        void AskForEmailValidation(string accountCreationStatus, string userEmail);

        [OperationContract]
        void GrantValidation(string validationStatus, AccountDTO accountInfoDTO);

        [OperationContract]
        void DeliverLobbyInvite(string lobbyCode);
    }
}
