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

        public GameHub(Game board)
        {
            _game = board;
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
            if (Game.Players.Where(x => x.Pseudo.Equals(pseudo)).Count().Equals(0))
            {
                Game.Players.Add(new Player(pseudo,Context.ConnectionId) { LastResponse = DateTime.Now });
                Game.CountPlayers();
                // si plus de 2 joueurs
                if(Game.Players.Count == 6)
                {
                    // si 6 joueurs
                    _timer.Stop();
                    Game.RedirectToGame();
                }
                else if(Game.Players.Count >=2)
                {
                    _timer.Interval = 3000;
                    _timer.Start();
                }

                //GameHub.Instance.Clients.Caller.userList(Game.Players.Count);
            }
            else
            {
                Player p = Game.Players.Find(x => x.Pseudo == pseudo);
                p.IdConnection = Context.ConnectionId;
            }
        }

        public void CreateGame(Object source, System.Timers.ElapsedEventArgs e)
        {
            Game.RedirectToGame();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Game.Players.Remove(Game.Players.Find(x => x.IdConnection == Context.ConnectionId));
            Game.CountPlayers();
            if (Game.Players.Count <= 1)
            {
                _timer.Stop();
            }
            return base.OnDisconnected(stopCalled);
        }


    }
}