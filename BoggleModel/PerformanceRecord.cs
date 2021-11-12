using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract(IsReference = true)]
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

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int WordsFound { get; private set; }

        [DataMember]
        public int DroppedMatches { get; private set; }

        [DataMember]
        public int WonMatches { get; private set; }

        [DataMember]
        public int LostMatches { get; private set; }

        [DataMember]
        public int PlayedMatches { get; private set; }

        [DataMember]
        public int HighestScore { get; private set; }

        [DataMember]
        public int TotalScore { get; private set; }

        [DataMember]
        [Key, ForeignKey("PlayerPerformance")]
        public string Username { get; set; }

        [DataMember]
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
