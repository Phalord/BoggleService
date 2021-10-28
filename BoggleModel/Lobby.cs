using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Lobby
    {
        private const string Public = "Public";
        private const string Private = "Private";

        public Lobby(string code, Player creator, string language)
        {
            Code = code;
            Size = 8;
            Privacy = Private;
            MessageHistory = new List<Message>();
            Players = new List<Player>();
            Players.Add(creator);
            GameMatch = new Match(language);
        }

        public string Code { get; private set; }

        public int Size { get; set; }

        public string Privacy { get; set; }

        public List<Message> MessageHistory { get; set; }

        public List<Player> Players { get; set; }

        public Match GameMatch { get; set; }

        public void ChangePrivacy()
        {
            if (Privacy.Equals(Public))
            {
                Privacy = Private;
            } else
            {
                Privacy = Public;
            }
        }
    }
}
