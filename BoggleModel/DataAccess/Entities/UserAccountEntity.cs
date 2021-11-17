using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Key]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsVerified { get; set; }

        public virtual PlayerEntity Player { get; set; }

    }
}
