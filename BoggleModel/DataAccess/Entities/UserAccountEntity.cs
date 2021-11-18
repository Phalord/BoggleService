using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoggleModel.DataAccess.Entities
{
    [Table("UserAccounts")]
    public class UserAccountEntity
    {
        public UserAccountEntity()
        {
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            IsVerified = false;
        }

        public UserAccountEntity(string userName,
            string email, string password, string friendCode)
        {
            UserName = userName;
            Email = email;
            Password = password;
            IsVerified = false;
            Player = new PlayerEntity(userName, friendCode, this);
        }

        [Key]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsVerified { get; set; }

        public virtual PlayerEntity Player { get; set; }

    }
}
