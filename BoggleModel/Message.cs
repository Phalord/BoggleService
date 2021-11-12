using System;
using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract]
    public class Message
    {
        public Message(string body)
        {
            Body = body;
            TimeSent = DateTime.Now;
        }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public DateTime TimeSent { get; set; }

        [DataMember]
        public Player Sender { get; set; }

        public void ChangeBody(string newBody)
        {
            Body = newBody;
        }
    }
}
