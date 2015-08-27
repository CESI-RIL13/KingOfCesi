using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class EventManager
    {
        public Dictionary<String,List<Card>> Events {get; set;}
        private static EventManager _eventManagerInstance;

        public static EventManager GetInstance()
        {
            if(_eventManagerInstance == null)
            {
                _eventManagerInstance = new EventManager();
            }
            return _eventManagerInstance;
        }

        public EventManager()
        {
            Events = new Dictionary<String, List<Card>>();
            int nbEnum = Enum.GetNames(typeof(EventEnum)).Length;
            while (nbEnum > 0)
            {
                nbEnum--;
                Events[Enum.GetName(typeof(EventEnum),nbEnum)]= new List<Card>();
            }
        }
        public void RaiseEvent(EventEnum typeEvent, Board board)
        {
            foreach (Card card in Events[Enum.GetName(typeof(EventEnum), typeEvent)])
            {
                card.ExecuteActions(typeEvent, board);
            }
        }
        public void SubscribeEvents(Card card,List<EventEnum> typeEvents)
        {
            foreach(EventEnum typeEvent in typeEvents)
            {
                Events[Enum.GetName(typeof(EventEnum), typeEvent)].Add(card);
            }
        }
        public void UnsubscribeEvents(Card card, List<EventEnum> typeEvents)
        {
            foreach (EventEnum typeEvent in typeEvents)
            {
                Events[Enum.GetName(typeof(EventEnum), typeEvent)].Remove(card);
            }
        }
    }
}
