using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public delegate void AskClientDelegate(Player p);

    public class Board
    {
        public int NbRound { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public Dictionary<EventEnum, List<Card>> Observers { get; set; }
        public AskClientDelegate AskClient;
        public bool EndOfTurn { get; set; }
        public int CountAnwser { get; set; }
        public int CountAnwserStandbyFor { get; set; }
        public bool YeahItsFinish;

        public Board()
        {
            NbRound = 0;
            Players = new List<Player>();
            Observers = new Dictionary<EventEnum, List<Card>>();
            EndOfTurn = false;
            CountAnwser = 0;
            CountAnwserStandbyFor = 0;
        }

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
            List<Player> playerAliveList = Players.Where(x=> x.Location != LocationEnum.CIMETARY_CESI).ToList();
            CurrentPlayer = playerAliveList.IndexOf(CurrentPlayer) + 1 == playerAliveList.Count ? playerAliveList[0] : playerAliveList[playerAliveList.IndexOf(CurrentPlayer) + 1];

            CurrentPlayer.Dices = new List<Dice>();
            
            if (CurrentPlayer.Location == LocationEnum.CESI_BAY || CurrentPlayer.Location == LocationEnum.CESI_CITY)
                CurrentPlayer.VictoryPoint += 2;

            CurrentPlayer.HasResolveDice = false;
            CurrentPlayer.NbLancer = 3;
            EndOfTurn = false;
            CountAnwser = 0;
            CountAnwserStandbyFor = 0;
        }

        public void AffectCityPlace(Player player)
        {
            if(Players.Count(x => x.Location == LocationEnum.CESI_CITY) == 0) {
                player.VictoryPoint++;
                player.Location = LocationEnum.CESI_CITY;
            } else if (Players.Count(x => x.Location == LocationEnum.CESI_BAY) == 0 && Players.Count > 4)
            {
                player.VictoryPoint++;
                player.Location = LocationEnum.CESI_BAY;
            }
        }

        public void DiceResolve()
        {
            if (CurrentPlayer.SelectedDices.Count != 6 || CurrentPlayer.HasResolveDice)
            {
                return;
            }

            CurrentPlayer.HasResolveDice = true;
            CurrentPlayer.NbLancer = 0;

            int coeur = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.LIFE);
            int griffe = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.ATTACK);
            int energie = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.ENERGY);
            int un = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.ONE);
            int deux = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.TWO);
            int trois = CurrentPlayer.SelectedDices.Count(x => x.ActiveFace == FaceEnum.THREE);
            
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
                        p.PrendreDegats(griffe);
                        if(p.Disconnected == false)
                        {
                            AskClient(p);
                        }
                    }
                }
                AffectCityPlace(CurrentPlayer);
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
        
        public bool IsDead(Player player)
        {
            return player.Location == LocationEnum.CIMETARY_CESI;
        }

        public void CheckWinner()
        {

            //Verif win
            //Winner par KO
            int countWinners = 0;
            
            List<Player> playerAliveList = Players.Where(x => x.Location != LocationEnum.CIMETARY_CESI).ToList();
            if (playerAliveList.Count == 1)
            {
                countWinners++;
                playerAliveList[0].KingOfCesi = true;
            }
            else if (playerAliveList.Count == 0)
            {
                YeahItsFinish = false;
            }
            //Winner par Victory Point
            foreach (Player p in playerAliveList)
            {
                if (p.VictoryPoint >= 20)
                {
                    countWinners++;
                    p.KingOfCesi = true;
                }
            }

            if (countWinners > 0)
            {
                YeahItsFinish = true; // c'est la selecao
            }
        }
    }
}
