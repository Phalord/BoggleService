using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract]
    public class Match
    {
        private const string Classic = "Classic";

        public Match(string language)
        {
            Rounds = 3;
            WinningScore = 0;
            SecondsPerRound = 180;
            GameMode = Classic;
            Language = language;
            MatchBoard = new Board(language);
        }

        [DataMember]
        public int Rounds { get; set; }

        [DataMember]
        public int WinningScore { get; set; }

        [DataMember]
        public int SecondsPerRound { get; set; }

        [DataMember]
        public string GameMode { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public Board MatchBoard { get; set; }
    }
}
