using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BoggleModel
{
 
    [DataContract(IsReference = true)]
    public class UserAccount
    {
        public UserAccount(string username,
            string email, string password, string friendCode)
        {
            UserName = username;
            Email = email;
            Password = password;
            IsVerified = false;
            PlayerAccount = new Player(username, friendCode, this);
        }

        public UserAccount()
        {
            UserName = "";
            Email = "";
            Password = "";
            IsVerified = false;
            PlayerAccount = new Player();
        }

        [DataMember]
        [Key]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
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
