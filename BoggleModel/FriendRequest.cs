using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract(IsReference = true)]
    public class FriendRequest
    {
        private const string Pendant = "Pendant";
        private const string Accepted = "Accepted";
        private const string Declined = "Declined";

        public FriendRequest()
        {
            IsAccepted = Pendant;
        }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string IsAccepted { get; private set; }

        [DataMember]
        public virtual Player Sender { get; set; }

        [DataMember]
        public virtual Player Receiver { get; set; }

        public void AcceptRequest()
        {
            IsAccepted = Accepted;
        }

        public void DeclineRequest()
        {
            IsAccepted = Declined;
        }
    }
}
