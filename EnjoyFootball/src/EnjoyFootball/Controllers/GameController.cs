using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using EnjoyFootball.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    public class GameController : Controller
    {
        FootballContext context;
        public static DataManager dataManager;
        UserManager<IdentityUser> userManager;
        public GameController(FootballContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            dataManager = new DataManager(context);
            this.userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index(int id)
        {
            GameDetails tempGame = dataManager.getMatchByID(id);
            return View(tempGame);
        }

     
        public IActionResult CreateGame()
        {
            var newGame = new CreateGameVM();
            newGame.StartTime = DateTime.Now;
            return View(newGame);
        }

        public IActionResult ListFieldName()
        {
            var model = dataManager.GetAllFieldNames();
            return Json(model);
        }
        [HttpPost]
        public IActionResult CreateGame(CreateGameVM createGameVm)
        {
            var userId = dataManager.GetUserId(User.Identity.Name);
            var result = dataManager.CreateGame(createGameVm, userId);
            if (result)
            {
            return RedirectToAction("Lobby","Game");
            }
            else
            {
                ViewData["CreateGame"] = "1";
                return View(createGameVm);
            }
        }

        public IActionResult addplayer(GameDetails gameDetails)
        {
            Player newPlayer = new Player();
            newPlayer.Nickname = User.Identity.Name;
            gameDetails.PlayerList.Add(newPlayer);
            dataManager.AddPlayerToGame(gameDetails.Id,newPlayer);

            return RedirectToAction("Index",new { id = gameDetails.Id });
        }
    }
}
