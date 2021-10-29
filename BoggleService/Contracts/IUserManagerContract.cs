using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoggleService.Contracts
{
    [ServiceContract(CallbackContract = typeof(IUserManagerCallback), SessionMode = SessionMode.Required)]
    public interface IUserManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void LogIn(string username, string password);

        [OperationContract(IsOneWay = true)]
        void CreateAccount(string username, string email, string password);
    }

    [ServiceContract]
    public interface IUserManagerCallback
    {
        [OperationContract]
        void GrantAccess(bool access);
    }
}
