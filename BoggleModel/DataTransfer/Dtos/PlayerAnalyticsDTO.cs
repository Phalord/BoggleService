using System.Runtime.Serialization;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class PlayerAnalyticsDTO
    {
        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public int WordsFound { get; set; }

        [DataMember]
        public int DroppedMatches { get; set; }

        [DataMember]
        public int WonMatches { get; set; }

        [DataMember]
        public int LostMatches { get; set; }

        [DataMember]
        public int PlayedMatches { get; set; }

        [DataMember]
        public int HighestScore { get; set; }

        [DataMember]
        public int TotalScore { get; set; }
    }
}
