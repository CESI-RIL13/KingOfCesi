using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Event
    {
        public Player Player { get; set; }
        public EventEnum Action { get; set; }
        public int Amount { get; set; }


    }
}
