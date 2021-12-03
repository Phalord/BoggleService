using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract]
    public class Lobby
    {
        private const string Public = "Public";
        private const string Private = "Private";

        public Lobby(
            string code, Player creator,
            string language, string privacy,
            string gameMode, int roomSize)
        {
            Code = code;
            Size = roomSize;
            Privacy = privacy;
            MessageHistory = new List<Message>();
            Players = new List<Player>();
            Players.Add(creator);
            GameMatch = new Match(language, gameMode);
        }

        [DataMember]
        public string Code { get; private set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public string Privacy { get; set; }

        [DataMember]
        public List<Message> MessageHistory { get; set; }

        [DataMember]
        public List<Player> Players { get; set; }

        [DataMember]
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
