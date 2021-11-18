﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BoggleModel
{
 
    public class UserAccount
    {
        public UserAccount(string userName,
            string email, string password, string friendCode)
        {
            UserName = userName;
            Email = email;
            Password = password;
            IsVerified = false;
            PlayerAccount = new Player(userName, friendCode, this);
        }

        public UserAccount()
        {
            UserName = "";
            Email = "";
            Password = "";
            IsVerified = false;
            PlayerAccount = new Player();
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
