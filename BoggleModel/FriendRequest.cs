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

        public void AcceptRequest()
        {
            IsAccepted = Accepted;
        }

        public void DeclineRequest()
        {
            IsAccepted = Declined;
        }

        public virtual Player Sender { get; set; }
        public virtual Player Receiver { get; set; }
    }
}
