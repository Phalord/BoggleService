using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class PerformanceRecord
    {
        public PerformanceRecord()
        {
            WordsFound = 0;
            DroppedMatches = 0;
            WonMatches = 0;
            LostMatches = 0;
            PlayedMatches = 0;
            HighestScore = 0;
            TotalScore = 0;
        }
        public int ID { get; set; }

        public int WordsFound { get; private set; }

        public int DroppedMatches { get; private set; }

        public int WonMatches { get; private set; }

        public int LostMatches { get; private set; }

        public int PlayedMatches { get; private set; }

        public int HighestScore { get; private set; }

        public int TotalScore { get; private set; }

        [Key, ForeignKey("PlayerPerformance")]
        public string Username { get; set; }
        
        public virtual Player PlayerPerformance { get; set; }

        public void IncreaseWordsFound(int wordsFound)
        {
            WordsFound += wordsFound;
        }

        public void IncreaseDroppedMatches()
        {
            DroppedMatches += 1;
        }

        public void IncreaseWonMatches()
        {
            WonMatches += 1;
        }

        public void IncreaseLostMatches()
        {
            LostMatches += 1;
        }

        public void IncreasePlayedMatches()
        {
            PlayedMatches += 1;
        }

        public void SetNewHighestScore(int newHighestScore)
        {
            HighestScore = newHighestScore;
        }

        public void IncreaseTotalScore(int score)
        {
            TotalScore += score;
        }
    }
}
