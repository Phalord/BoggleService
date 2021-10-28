using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Lobby
    {
        public string Code { get; private set; }

        public string Size { get; set; }

        public string Privacy { get; set; }

        public List<Message> MessageHistory { get; set; }

        public List<Player> Players { get; set; }

        public Match GameMatch { get; set; }
    }
}
