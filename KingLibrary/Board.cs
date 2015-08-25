using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public delegate void AskClientDelegate();

    public class Board
    {
        public int NbRound { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public Dictionary<EventEnum, List<Card>> Observers { get; set; }
        public List<Player> playerTokyo { get; set; }
        public AskClientDelegate AskClient;

        public Board()
        {
            NbRound = 0;
            Players = new List<Player>();
            Observers = new Dictionary<EventEnum, List<Card>>();
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

        public void NextPlayer()
        {
           for(int i = 0; i < Players.Count; i++)
            {
                if(Players[i].Pseudo == CurrentPlayer.Pseudo)
                {
                    CurrentPlayer.listededes = new List<Dice>();
                    CurrentPlayer = (i+1 == Players.Count ? Players[0] : Players[i + 1]);

                    if (CurrentPlayer.Location == LocationEnum.CESI_BAY || CurrentPlayer.Location == LocationEnum.CESI_CITY)
                        CurrentPlayer.VictoryPoint += 2;

                    CurrentPlayer.NbLancer = 3;
                    break;
                }
            }
        }

        public void DiceResolve()
        {
            int coeur = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.LIFE);
            int griffe = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ATTACK);
            int energie = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ENERGY);
            int un = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ONE);
            int deux = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.TWO);
            int trois = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.THREE);
            
            CurrentPlayer.Energy += energie;

            if(CurrentPlayer.Location == LocationEnum.OUT_CESI)
                CurrentPlayer.Soingner(coeur);
            
            if (griffe > 0)
            {
                if(CurrentPlayer.Location == LocationEnum.CESI_CITY || CurrentPlayer.Location == LocationEnum.CESI_BAY)
                {
                    foreach (Player p in Players.Where(x=> x.Location == LocationEnum.OUT_CESI))
                    {
                        p.PrendreDegats(griffe);
                    }
                }
                else
                {
                    foreach (Player p in Players.Where(x => x.Location == LocationEnum.CESI_BAY || x.Location == LocationEnum.CESI_CITY))
                    {
                        AskClient();
                        p.PrendreDegats(griffe);
                    }
                }
                if (Players.Count(x => x.Location == LocationEnum.CESI_CITY) == 0)
                {
                    CurrentPlayer.VictoryPoint += 1;
                    CurrentPlayer.Location = LocationEnum.CESI_CITY;
                }
                else if (Players.Count(x => x.Location == LocationEnum.CESI_CITY) != 0 && Players.Count(x => x.Location == LocationEnum.CESI_BAY) == 0 && Players.Count>4)
                {
                    CurrentPlayer.VictoryPoint += 1;
                    CurrentPlayer.Location = LocationEnum.CESI_BAY;
                }
            }
            if (un >= 3)
            {
                CurrentPlayer.GainVPWithDices(1, un);
            }
            if (deux >= 3)
            {
                CurrentPlayer.GainVPWithDices(2, deux);
            }
            if (trois >= 3)
            {
                CurrentPlayer.GainVPWithDices(3, trois);
            }
        }  
              
    }
}
