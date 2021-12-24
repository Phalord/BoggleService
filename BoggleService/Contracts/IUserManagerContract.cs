using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
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
