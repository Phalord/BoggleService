using System.Runtime.Serialization;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class PlayerOverviewDTO
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public int Victories { get; set; }

        [DataMember]
        public int GamesPlayed { get; set; }

        [DataMember]
        public int HighestScore { get; set; }

        [DataMember]
        public int TotalScore { get; set; }
    }
}
