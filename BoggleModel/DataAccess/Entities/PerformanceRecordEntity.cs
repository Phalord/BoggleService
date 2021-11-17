using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoggleModel.DataAccess.Entities
{
    [Table("PerformanceRecords")]
    public class PerformanceRecordEntity
    {
        public PerformanceRecordEntity()
        {
            WordsFound = 0;
            DroppedMatches = 0;
            WonMatches = 0;
            LostMatches = 0;
            PlayedMatches = 0;
            HighestScore = 0;
            TotalScore = 0;
        }

        [Key, ForeignKey(nameof(Player))]
        public string Username { get; set; }

        public int WordsFound { get; set; }

        public int DroppedMatches { get; set; }

        public int WonMatches { get; set; }

        public int LostMatches { get; set; }

        public int PlayedMatches { get; set; }

        public int HighestScore { get; set; }

        public int TotalScore { get; set; }

        public virtual PlayerEntity Player { get; set; }
    }
}