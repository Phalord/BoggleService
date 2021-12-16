using System;
using System.Runtime.Serialization;

namespace BoggleModel
{
    [DataContract]
    public class Message
    {
        public Message(string body, string sender)
        {
            Body = body;
            Sender = sender;
            TimeSent = DateTime.Now;
        }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public DateTime TimeSent { get; set; }

        [DataMember]
        public string Sender { get; set; }

        public void ChangeBody(string newBody)
        {
            Body = newBody;
        }
    }
}
