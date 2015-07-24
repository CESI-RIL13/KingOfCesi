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
        private readonly static Lazy<GameHub> _instance = new Lazy<GameHub>(
        () => new GameHub());
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
                return _instance.Value;
            }
        }
        public void Join(string pseudo)
        {
            if (Game.KingBoard.Players.Where(x => x.Pseudo.Equals(pseudo)).Count().Equals(0))
            {
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
                    _timer.Interval = 3000;
                    _timer.Start();
                }
                //GameHub.Instance.Clients.Caller.userList(Game.Players.Count);
            }
            else
            {
                Player p = Game.KingBoard.Players.Find(x => x.Pseudo == pseudo);
                p.IdConnection = Context.ConnectionId;
            }

            Game.UpdateBoard();
        }

        public void CreateGame(Object source, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            Game.KingBoard.NbRound =1;
            Game.UpdateBoard();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Game.KingBoard.Players.Remove(Game.KingBoard.Players.Find(x => x.IdConnection == Context.ConnectionId));
            Game.CountPlayers();
            if (Game.KingBoard.Players.Count <= 1)
            {
                _timer.Stop();
            }
            return base.OnDisconnected(stopCalled);
        }


    }
}