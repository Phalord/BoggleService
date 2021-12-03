using System.Runtime.Serialization;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class PublicLobbyPreviewDTO
    {
        [DataMember]
        public int LobbySize { get; set; }

        [DataMember]
        public int PlayersInside { get; set; }

        [DataMember]
        public string GameMode { get; set; }

        [DataMember]
        public string LobbyCode { get; set; }
    }
}
