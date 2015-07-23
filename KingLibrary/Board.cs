using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Board
    {
        public static int NbRound { get; set; }
        public static List<Player> Players { get; set; }
        public static Player CurentPlayer { get; set; }
        public static Dictionary<EventEnum, List<Card>> Observers { get; set; }
        private readonly static Lazy<Board> _instance = new Lazy<Board>(() => new Board());

        public Board()
        {
            NbRound = 0;
            Players = new List<Player>();
            Observers = new Dictionary<EventEnum, List<Card>>();
        }

        public static Board Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// Ajoute un joueur.
        /// </summary>
        /// <param name="name">Le nom du joueur.</param>
        //public void AddPlayer(string name) {
        //    Players.Add(new Player(name),);
        //}

        /// <summary>
        /// Supprime un joueur.
        /// </summary>
        /// <param name="player">Le joueur.</param>
        //public void RemovePlayer(Player player)
        //{
        //    Players.Remove(player);
        //}

        public void SubscribeEvent(EventEnum eventParam, Card card)
        {
            if (!Observers.ContainsKey(eventParam))
            {
                Observers[eventParam] = new List<Card>();
            }
            Observers[eventParam].Add(card);
        }

        public void UnsubscribeEvent(Card card)
        {
            foreach (KeyValuePair<EventEnum, List<Card>> keyValuePair in Observers)
            {
                keyValuePair.Value.Remove(card);
            }
        }
    }
}
