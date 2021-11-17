using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoggleModel.DataAccess.Entities
{
    [Table("Players")]
    public class PlayerEntity
    {
        public PlayerEntity()
        {
            UserName = string.Empty;
            Nickname = string.Empty;
            FriendCode = string.Empty;
            Nationality = string.Empty;
            Status = string.Empty;
            FriendRequests = new List<FriendRequestEntity>();
        }

        [Key, ForeignKey(nameof(Account))]
        public string UserName { get; set; }

        public string Nickname { get; set; }

        public string FriendCode { get; set; }

        public string Nationality { get; set; }

        public string Status { get; set; }

        public UserAccountEntity Account { get; set; }

        public PerformanceRecordEntity PerformanceRecord { get; set; }

        public List<FriendRequestEntity> FriendRequests { get; set; }
    }
}