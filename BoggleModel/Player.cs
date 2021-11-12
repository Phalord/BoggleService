using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract(IsReference = true)]
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

        [DataMember]
        [Key, ForeignKey("Account")]
        public string UserName { get; set; }

        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public string FriendCode { get; private set; }

        [DataMember]
        public string Nationality { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public virtual UserAccount Account { get; set; }

        [DataMember]
        public virtual PerformanceRecord Performance { get; set; }

        [DataMember]
        public virtual List<FriendRequest> FriendRequests { get; set; }
    }
}
