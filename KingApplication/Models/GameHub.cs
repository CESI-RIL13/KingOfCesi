using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using KingLibrary;
using System.Threading.Tasks;
using System.Timers;

namespace WebApplication1.Models
{
    [HubName("gameHub")]
    public class GameHub : Hub
    {
        /* private readonly static Lazy<GameHub> _instance = new Lazy<GameHub>(
         () => new GameHub()); */
        private static GameHub _instance = null;
        private readonly Game _game;
        private Timer _timer;
        public IHubContext _context;

        public GameHub() : this(Game.Instance) { }

        public GameHub(Game game)
        {
            _game = game;
            _timer = new Timer();
            _timer.Elapsed += CreateGame;
            _context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
        }

        public static GameHub Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameHub();

                return _instance;
            }
        }
        public void Join(string pseudo)
        {
            if (Game.KingBoard.Players.Where(x => x.Pseudo.Equals(pseudo)).Count().Equals(0))
            {
                if(Game.KingBoard.NbRound > 0)
                {
                    GameHub.Instance._context.Clients.Client(Context.ConnectionId).redirectToLobby();
                    return;
                }
                Game.KingBoard.Players.Add(new Player(pseudo,Context.ConnectionId) { LastResponse = DateTime.Now });
                // si plus de 2 joueurs
                if (Game.KingBoard.Players.Count == 6)
                {
                    // si 6 joueurs
                    _timer.Stop();
                    Game.KingBoard.NbRound = 1;
                }
                else if(Game.KingBoard.Players.Count >=2)
                {
                    _timer.Interval = 30000;
                    _timer.Start();
                }
                //GameHub.Instance.Clients.Caller.userList(Game.Players.Count);
            }
            else
            {
                Player p = Game.KingBoard.Players.Find(x => x.Pseudo == pseudo);
                p.Disconnected = false;
                p.IdConnection = Context.ConnectionId;
            }

            Game.UpdateBoard();
        }

        public void CreateGame(Object source, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            Game.KingBoard.NbRound = 1;
            Game.KingBoard.CurrentPlayer = Game.KingBoard.Players[0];
            Game.KingBoard.CurrentPlayer.NbLancer = 3;

            for(int i = 0; i < Game.KingBoard.Players.Count; i++)
            {
                Game.KingBoard.Players[i].Monster = (MonsterEnum)i;
            }

            Game.UpdateBoard();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Player p = Game.KingBoard.Players.FirstOrDefault(x => x.IdConnection == Context.ConnectionId);
            if (p != null)
            {
                p.Disconnected = true;
            }
            Game.UpdateBoard();
            Game.CountPlayers();
            if (Game.KingBoard.Players.Count <= 1)
            {
                _timer.Stop();
            }
            return base.OnDisconnected(stopCalled);
        }

        public void DiceResolve()
        {
            if (CheckCurrentPlayer())
            {
                /*NINJA!*/
                Game.KingBoard.AskClient += AskClient;
                Game.KingBoard.DiceResolve(Context.ConnectionId);

                Game.KingBoard.AskClient -= AskClient;
                if (Game.KingBoard.CountAnwserStandbyFor == 0)
                {
                    Game.KingBoard.EndOfTurn = true;
                    Game.UpdateBoard();
                }
            }
        }


        public static void AskClient(Player player, string contextConnectionId)
        {
            if (player != null && player.Location != LocationEnum.CIMETARY_CESI)
            {
                Game.KingBoard.CountAnwserStandbyFor++;
                GameHub.Instance._context.Clients.Client(player.IdConnection).askPlayer();
                GameHub.Instance._context.Clients.Client(contextConnectionId).waitForTokyoAnswer();
            }
        }

        public void EndOfTurn()
        {
            if (CheckCurrentPlayer())
            {
                /*NINJA!*/
                Game.KingBoard.CurrentPlayer.SelectedDices.Clear();
                Game.KingBoard.CheckWinner();

                Game.KingBoard.NextPlayer();
                Game.KingBoard.NbRound++;
                Game.KingBoard.EventManager.RaiseEvent(EventEnum.END_ROUND, Game.KingBoard);
                Game.UpdateBoard();
            }
        }

        public void PlayerRollDices()
        {
            if (CheckCurrentPlayer())
            {
                Game.KingBoard.CurrentPlayer.ThrowDices();
                Game.KingBoard.EventManager.RaiseEvent(EventEnum.ROLL, Game.KingBoard);
                Game.UpdateBoard();
            }
        }

        public void SelectDice(int position)
        {
            if (CheckCurrentPlayer())
            {
                Game.KingBoard.CurrentPlayer.SelectDice(position);
                Game.KingBoard.EventManager.RaiseEvent(EventEnum.KEEP_DICE, Game.KingBoard);
                Game.UpdateBoard();
            }
        }

        public void UnselectDice(int position)
        {
           if (CheckCurrentPlayer())
            {
                Game.KingBoard.CurrentPlayer.UnselectDice(position);
                Game.UpdateBoard();
            }
        }
        private bool CheckCurrentPlayer()
        {
            Player p = Game.KingBoard.Players.Find(x => x.IdConnection == Context.ConnectionId);
            return p.Pseudo == Game.KingBoard.CurrentPlayer.Pseudo;
        }

        public void LeaveTokyo(bool answer)
        {
            Player p = Game.KingBoard.Players.Find(x => x.IdConnection == Context.ConnectionId && (x.Location == LocationEnum.CESI_BAY || x.Location == LocationEnum.CESI_CITY));
            Game.KingBoard.CountAnwser++;
            if (answer && p != null)
            {
                Game.KingBoard.EventManager.RaiseEvent(EventEnum.LEAVE_TOKYO,Game.KingBoard);
                p.Location = LocationEnum.OUT_CESI;

                if (Game.KingBoard.CurrentPlayer.Location != LocationEnum.CESI_CITY && Game.KingBoard.CurrentPlayer.Location != LocationEnum.CESI_BAY)
                { 
                    Game.KingBoard.AffectCityPlace(Game.KingBoard.CurrentPlayer);
                }
            }
            if (Game.KingBoard.CountAnwser == Game.KingBoard.CountAnwserStandbyFor)
            {
                Game.KingBoard.EndOfTurn = true;
            }
            Game.UpdateBoard();
        }

        public void BuyCard(string cardName)
        {
            if(CheckCurrentPlayer())
            {
                Card card = Game.KingBoard.Deck.FirstOrDefault(x => x.SafeName == cardName);
                if(card != null)
                {
                    if(card.Cost <= Game.KingBoard.CurrentPlayer.Energy)
                    {
                        card.Owner = Game.KingBoard.CurrentPlayer;
                        Game.KingBoard.CurrentPlayer.MyCards.Add(card);
                        Game.KingBoard.CurrentPlayer.ImpactEnergy(-(card.Cost));
                        Game.KingBoard.Deck.Remove(card);
                        List<EventEnum> eventActions = new List<EventEnum>();
                        foreach (CardAction cardAction in card.CardActions)
                        {
                            eventActions.Add(cardAction.TypeEvent);
                        }
                        Game.KingBoard.EventManager.SubscribeEvents(card, eventActions);
                        Game.KingBoard.EventManager.RaiseEvent(EventEnum.CARD_BOUGHT, Game.KingBoard);
                        this._context.Clients.Client(Context.ConnectionId).resultBuyCard(true);
                    }
                    else
                        this._context.Clients.Client(Context.ConnectionId).resultBuyCard(false);
                }
                else
                    this._context.Clients.Client(Context.ConnectionId).resultBuyCard(false);
            }
            else
                this._context.Clients.Client(Context.ConnectionId).resultBuyCard(false);
        }

        public void RefreshBoard()
        {
            Game.UpdateBoard();
        }
    }
}