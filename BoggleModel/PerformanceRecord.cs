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
            Nickname = "";
        }

        public int WordsFound { get; set; }

        public int DroppedMatches { get; set; }

        public int WonMatches { get; set; }

        public int LostMatches { get; set; }

        public int PlayedMatches { get; set; }

        public int HighestScore { get; set; }

        public int TotalScore { get; set; }

        public string Nickname { get; set; }

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
