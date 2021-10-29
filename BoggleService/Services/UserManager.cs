using BoggleService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoggleService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserManager : IUserManagerContract
    {

        public void CreateAccount(string username, string email, string password)
        {
            
        }

        public void LogIn(string username, string password)
        {
            Callback.GrantAccess(username.Equals("phalord")
                && password.Equals("123456"));
        }

        IUserManagerCallback Callback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IUserManagerCallback>();
            }
        }
    }
}
