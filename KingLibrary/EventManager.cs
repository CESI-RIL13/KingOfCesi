using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    class EventManager
    {
        public Dictionary<EventEnum,List<Card>> Events {get; set;}

        public void RaiseEvent(EventEnum typeEvent, Board board)
        {
            foreach (Card card in Events[typeEvent])
            {
                card.ExecuteActions(typeEvent, board);
            }
        }
        public void SubscribeEvents(Card card,List<EventEnum> typeEvents)
        {
            foreach(EventEnum typeEvent in typeEvents)
            {
                Events[typeEvent].Add(card);
            }
        }
        public void UnsubscribeEvents(Card card, List<EventEnum> typeEvents)
        {
            foreach (EventEnum typeEvent in typeEvents)
            {
                Events[typeEvent].Remove(card);
            }
        }
    }
}
