using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingLibrary;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Loading()
        {
            return View();
        }

        public ActionResult Play()
        {
            return View();
        }

        public ActionResult Win()
        {
            return View();
        }

        public ActionResult Loose()
        {
            return View();
        }
    }
}