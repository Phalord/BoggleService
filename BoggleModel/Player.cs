using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Player
    {
        private const string Offline = "Offline";
        private const string Online = "Online";
        private const string InLobby = "In Lobby";
        private const string InGame = "In Game";

        public Player()
        {
            Nickname = string.Empty;
            Nationality = string.Empty;
            FriendCode = string.Empty;
            Status = Offline;
            Performance = new PerformanceRecord();
            FriendRequests = new List<FriendRequest>();
        }

        public Player(string nickname, string friendCode, UserAccount userAccount)
        {
            Nickname = nickname;
            Nationality = string.Empty;
            FriendCode = friendCode;
            Status = Offline;
            Account = userAccount;
            Performance = new PerformanceRecord();
            FriendRequests = new List<FriendRequest>();
        }

        public string Nickname { get; set; }

        public string FriendCode { get; private set; }
        
        public string Nationality { get; set; }

        public string Status { get; set; }

        [Key, ForeignKey("Account")]
        public string UserName { get; set; }

        public virtual UserAccount Account { get; set; }

        public virtual PerformanceRecord Performance { get; set; }

        public virtual List<FriendRequest> FriendRequests { get; set; }
    }
}
