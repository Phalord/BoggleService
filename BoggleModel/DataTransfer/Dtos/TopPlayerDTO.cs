using System.Runtime.Serialization;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class TopPlayerDTO
    {
        [DataMember]
        public string PlayerNickname { get; set; }

        [DataMember]
        public int PlayedMatches { get; set; }

        [DataMember]
        public int WonMatches { get; set; }

        [DataMember]
        public int TotalScore { get; set; }
    }
}
