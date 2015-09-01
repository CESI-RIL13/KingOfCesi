using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace KingLibrary
{
    public delegate void AskClientDelegate(Player p, string contextConnectionId);

    public class Board
    {
        public int NbRound { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        [ScriptIgnore]
        public AskClientDelegate AskClient;
        public bool EndOfTurn { get; set; }
        public int CountAnwser { get; set; }
        public int CountAnwserStandbyFor { get; set; }
        public bool YeahItsFinish;
        [ScriptIgnore]
        public EventManager EventManager { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> CardsShop
        {
            get
            {
                return Deck.Take(3).ToList();
            }
        }

        public Board()
        {
            NbRound = 0;
            Players = new List<Player>();
            EndOfTurn = false;
            CountAnwser = 0;
            CountAnwserStandbyFor = 0;
            EventManager = new EventManager();
            Deck = CardFactory.GenerateDeck();
        }
        public void NextPlayer()
        {
            List<Player> playerAliveList = Players.Where(x => x.Location != LocationEnum.CIMETARY_CESI).ToList();
            CurrentPlayer = playerAliveList.IndexOf(CurrentPlayer) + 1 == playerAliveList.Count ? playerAliveList[0] : playerAliveList[playerAliveList.IndexOf(CurrentPlayer) + 1];
            CurrentPlayer.Dices = new List<Dice>();

            if (CurrentPlayer.Location == LocationEnum.CESI_BAY || CurrentPlayer.Location == LocationEnum.CESI_CITY) {
                CurrentPlayer.VictoryPoint += 2;
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
            }


            CurrentPlayer.HasResolveDice = false;
            CurrentPlayer.NbLancer = CurrentPlayer.NbLancerMax;
            EndOfTurn = false;
            CountAnwser = 0;
            CountAnwserStandbyFor = 0;

            EventManager.RaiseEvent(EventEnum.BEGIN_ROUND, this);
        }

        public void AffectCityPlace(Player player)
        {
            if(Players.Count(x => x.Location == LocationEnum.CESI_CITY) == 0) {
                player.VictoryPoint++;
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
                player.Location = LocationEnum.CESI_CITY;
                EventManager.RaiseEvent(EventEnum.ENTER_TOKYO, this);
            }
            else if (Players.Count(x => x.Location == LocationEnum.CESI_BAY) == 0 && Players.Count > 4)
            {
                player.VictoryPoint++;
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
                player.Location = LocationEnum.CESI_BAY;
                EventManager.RaiseEvent(EventEnum.ENTER_TOKYO, this);
            }
        }

        public void DiceResolve(string contextConnectionId)
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
            if(energie > 0)
                EventManager.RaiseEvent(EventEnum.GAIN_ENERGY, this);


            if (CurrentPlayer.Location == LocationEnum.OUT_CESI)
            {
                CurrentPlayer.Soigner(coeur);
                EventManager.RaiseEvent(EventEnum.GAIN_HP, this);
            }
            
            if (griffe > 0)
            {
                if(CurrentPlayer.Location == LocationEnum.CESI_CITY || CurrentPlayer.Location == LocationEnum.CESI_BAY)
                {
                    foreach (Player p in Players.Where(x=> x.Location == LocationEnum.OUT_CESI))
                    {
                        p.PrendreDegats(griffe);
                        if(p.Hp == 0)
                        {
                            EventManager.RaiseEvent(EventEnum.ON_DEATH, this);
                        }
                        EventManager.RaiseEvent(EventEnum.HP_LOSS, this);
                    }
                }
                else
                {
                    foreach (Player p in Players.Where(x => x.Location == LocationEnum.CESI_BAY || x.Location == LocationEnum.CESI_CITY))
                    {
                        p.PrendreDegats(griffe);
                        if (p.Hp == 0)
                        {
                            EventManager.RaiseEvent(EventEnum.ON_DEATH, this);
                        }
                        EventManager.RaiseEvent(EventEnum.HP_LOSS, this);
                        if (p.Disconnected == false)
                        {

                            AskClient(p, contextConnectionId);
                        }
                    }
                }
                AffectCityPlace(CurrentPlayer);
            }
            if (un >= 3)
            {
                CurrentPlayer.GainVPWithDices(1, un);
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
            }
            if (deux >= 3)
            {
                CurrentPlayer.GainVPWithDices(2, deux);
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
            }
            if (trois >= 3)
            {
                CurrentPlayer.GainVPWithDices(3, trois);
                EventManager.RaiseEvent(EventEnum.GAIN_VICTORYPOINT, this);
            }

            EventManager.RaiseEvent(EventEnum.RESOLVE_DICE, this);
        }

        public bool IsDead(Player player)
        {
            return player.Location == LocationEnum.CIMETARY_CESI;
        }

        public List<Player> PlayersAlives()
        {
            return Players.Where(x => x.Location != LocationEnum.CIMETARY_CESI).ToList();
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
