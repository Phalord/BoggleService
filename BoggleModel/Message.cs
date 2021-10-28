using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Message
    {
        public string Body { get; set; }

        public DateTime TimeSent { get; set; }

        public Player Sender { get; set; }
    }
}
