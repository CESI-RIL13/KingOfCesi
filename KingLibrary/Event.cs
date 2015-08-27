using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Event
    {
        public EventEnum Action { get; set; }
        public List<Card> Cards { get; set; }

        public void RaiseEvents()
        {
            foreach(Card card in Cards)
            {
                card.ExecuteActions();
            }
        }
    }
}
