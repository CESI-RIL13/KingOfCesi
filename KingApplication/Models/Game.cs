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
        public static List<Player> Players { get; set; }

        public Game()
        {
            Players = new List<Player>();
        }

        public static Game Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public static void GetPlayers()
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(Players);

            var clients = GameHub.Instance._context.Clients.All;
            clients.userList(sJSON);
        }

        public static void CountPlayers()
        {
            GameHub.Instance._context.Clients.All.userCount(Players.Count);
        }

        public static void RedirectToGame()
        {
            GameHub.Instance._context.Clients.All.redirectToGame("/Game/");
        }
    }
}