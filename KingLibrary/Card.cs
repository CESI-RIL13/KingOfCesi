using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace KingLibrary
{

    public class Card
    {
        public string Name { get; set; }
        public string SafeName {
            get {
                return Name.Replace(' ', '-');
            }
        }
        public int Cost { get; set; }
        [ScriptIgnore]
        public List<CardAction> CardActions { get; set; }
        [ScriptIgnore]
        public Player Owner { get; set; }

        public Card(string name, int cost, List<CardAction> cardsActions, Player owner)
        {
            this.Name = name;
            this.Cost = cost;
            this.CardActions = cardsActions;
            this.Owner = owner;
        }

        public void ExecuteActions(EventEnum typeEvent, Board board)
        {
            foreach (CardAction cardAction in CardActions)
            {
                if(cardAction.TypeEvent == typeEvent)
                {
                    cardAction.Execute(board);
                }
            }
        }

    }
}
