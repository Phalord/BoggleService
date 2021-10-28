using System;
using System.Collections.Generic;
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

        public Player(string nickname, string friendCode)
        {
            Nickname = nickname;
            Nationality = string.Empty;
            FriendCode = friendCode;
            Status = Offline;
        }

        public string Nickname { get; set; }

        public string FriendCode { get; private set; }
        
        public string Nationality { get; set; }

        public string Status { get; set; }

        public virtual UserAccount Account { get; set; }

        public virtual PerformanceRecord Performance { get; set; }
    }
}
