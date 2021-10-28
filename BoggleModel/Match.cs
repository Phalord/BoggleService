using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Match
    {
        public int Rounds { get; set; }

        public int WinningScore { get; set; }

        public int SecondsPerRound { get; set; }

        public string GameMode { get; set; }

        public string Language { get; set; }

        public Board MatchBoard { get; set; }
    }
}
