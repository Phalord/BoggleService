
namespace BoggleModel
{
    public class FriendRequest
    {
        private const string Pendant = "Pendant";
        private const string Accepted = "Accepted";
        private const string Declined = "Declined";

        public FriendRequest()
        {
            IsAccepted = Pendant;
        }

        public string IsAccepted { get; private set; }

        public Player Sender { get; set; }

        public Player Receiver { get; set; }

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
