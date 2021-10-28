using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel
{
    public class Player
    {
        public string Nickname { get; set; }

        public string FriendCode { get; private set; }
        
        public string Nationality { get; set; }
    }
}
