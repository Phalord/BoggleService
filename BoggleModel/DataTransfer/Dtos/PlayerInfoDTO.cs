using System.Runtime.Serialization;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class PlayerInfoDTO
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public string Nationality { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }
    }
}
