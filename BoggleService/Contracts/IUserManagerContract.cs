using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoggleService.Contracts
{
    [ServiceContract(CallbackContract = typeof(IUserManagerCallback),
        SessionMode = SessionMode.Required)]
    public interface IUserManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void LogIn(string userName, string password);

        [OperationContract(IsOneWay = true)]
        void CreateAccount(string userName, string email, string password);

        [OperationContract(IsOneWay = true)]
        void ValidateEmail(string validationCode, string email);
    }

    [ServiceContract]
    public interface IUserManagerCallback
    {
        [OperationContract]
        void GrantAccess(string accessStatus);

        [OperationContract]
        void AskForEmailValidation(string accountCreationStatus, string userEmail);

        [OperationContract]
        void GrantValidation(string validationStatus);
    }
}
