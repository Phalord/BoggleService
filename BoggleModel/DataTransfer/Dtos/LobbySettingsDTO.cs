using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel.DataTransfer.Dtos
{
    [DataContract]
    public class LobbySettingsDTO
    {

        [DataMember]
        public int NumberOfPlayers { get; set; }

        [DataMember]
        public string GameMode { get; set; }

        [DataMember]
        public string LobbyPrivacy { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string CreatorUserName { get; set; }
    }
}
