using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class PerformanceRecord
    {
        public int WordsFound { get; set; }

        public int MatchesDropped { get; set; }

        public int MatchesWon { get; set; }

        public int MatchesLost { get; set; }

        public int MatchesPlayed { get; set; }

        public int HighestScore { get; set; }

        public int TotalScore { get; set; }
    }
}
