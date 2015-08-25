using KingLibrary;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Game
    {
        private readonly static Lazy<Game> _instance = new Lazy<Game>(() => new Game());
        public static Board KingBoard { get; set; }

        public Game()
        {
            KingBoard = new Board();
        }

        public static Game Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public static void UpdateBoard()
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            
            string sJSON = oSerializer.Serialize(KingBoard);

            var clients = GameHub.Instance._context.Clients.All;
            clients.updateBoard(sJSON);
        }

        public static void AskClient()
        {
            Player p = Game.KingBoard.Players.FirstOrDefault(x => x.Location == LocationEnum.CESI_CITY);
            if (p != null)
            {
                GameHub.Instance._context.Clients.Client(p.IdConnection).askPlayer();
            }

            //Player p = Game.KingBoard.Players.Find(x => x.IdConnection == GameHub.Instance.Context.ConnectionId);
        }

        public static void CountPlayers()
        {
            GameHub.Instance._context.Clients.All.userCount(KingBoard.Players.Count);
        }

        

    }
}