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

        public PlayerEntity(string userName,
            string friendCode, UserAccountEntity userAccountEntity)
        {
            UserName = userName;
            Nationality = string.Empty;
            FriendCode = friendCode;
            Status = "Offline";
            Account = userAccountEntity;
            PerformanceRecord = new PerformanceRecordEntity();
            FriendRequests = new List<FriendRequestEntity>();
        }

        [Key, ForeignKey(nameof(Account))]
        public string UserName { get; set; }

        public string Nickname { get; set; }

        public string FriendCode { get; set; }

        public string Nationality { get; set; }

        public string Status { get; set; }

        public virtual UserAccountEntity Account { get; set; }

        public virtual PerformanceRecordEntity PerformanceRecord { get; set; }

        public virtual List<FriendRequestEntity> FriendRequests { get; set; }
    }
}