using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
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

        public int Rounds { get; set; }

        public int WinningScore { get; set; }

        public int SecondsPerRound { get; set; }

        public string GameMode { get; set; }

        public string Language { get; set; }

        public Board MatchBoard { get; set; }
    }
}
