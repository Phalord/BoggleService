using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class UserAccount
    {
        public UserAccount(string username, string email, string password, string friendCode)
        {
            UserName = username;
            Email = email;
            Password = password;
            IsVerified = false;
            PlayerAccount = new Player(username, friendCode);
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsVerified { get; set; }

        public virtual Player PlayerAccount { get; set; }

        public void Verify()
        {
            if (!IsVerified)
            {
                IsVerified = true;
            }
        }

    }
}
