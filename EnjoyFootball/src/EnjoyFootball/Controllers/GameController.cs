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
        DataManager dataManager;
        UserManager<IdentityUser> userManager;
        public GameController(FootballContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            dataManager = new DataManager();
            this.userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index(int id)
        {
            GameDetails tempGame = dataManager.getGameByID(id);
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

        public IActionResult Addplayer(GameDetails gameDetails)
        {
            dataManager.AddPlayerToGame(User.Identity.Name, gameDetails.Id);

            return RedirectToAction("Index",new { id = gameDetails.Id });
        }

        public IActionResult RemovePlayer(GameDetails gameDetails)
        {
            //var player= gameDetails.PlayerList
            //    .Where(o => o.Nickname == User.Identity.Name)
            //    .First();
            //gameDetails.PlayerList.Remove(player, User.Identity.Name);

            dataManager.RemovePlayerFromGame(gameDetails.Id, User.Identity.Name);

            return RedirectToAction("Index", new { id = gameDetails.Id });
        }
    }
}
