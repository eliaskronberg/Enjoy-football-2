using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using EnjoyFootball.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    
    [Authorize]
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
        public async Task<IActionResult> Index(int id)
        {
            GameDetailsVM tempGame = await dataManager.getGameByID(id);
            tempGame.CurrentPlayerId = await dataManager.GetSingleUserId(User.Identity.Name);
            var allField = await dataManager.ListFields();
            var theFieldCoord= allField.Where(o => o.Name == tempGame.Field).Select(o => o.Coordinates).SingleOrDefault();
            tempGame.FieldId = theFieldCoord;
            return View(tempGame);
        }

     
        public IActionResult CreateGame()
        {
            var newGame = new CreateGameVM();
            newGame.StartTime = DateTime.Now;
            return View(newGame);
        }

        public async Task<IActionResult> ListFieldName()
        {
            var model = await dataManager.GetAllFieldNames();
            return Json(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateGame(CreateGameVM createGameVm)
        //{
        //    var userId = await dataManager.GetSingleUserId(User.Identity.Name);
        //    var newGameId = await dataManager.CreateGame(createGameVm, userId);
        //    if (newGameId != 0)
        //    {
        //        return RedirectToAction("game", "Index", newGameId);
        //    }
        //    else
        //        return RedirectToAction("game", "CreateGame");
           
        //}

        public IActionResult Addplayer(GameDetailsVM gameDetails)
        {
            dataManager.AddPlayerToGame(gameDetails.CurrentPlayerId, gameDetails.Id);

            return RedirectToAction("Index",new { id = gameDetails.Id });
        }

        public IActionResult RemovePlayer(string UserId, int GameId)
        {
            //var player= gameDetails.PlayerList
            //    .Where(o => o.Nickname == User.Identity.Name)
            //    .First();
            //gameDetails.PlayerList.Remove(player, User.Identity.Name);

            dataManager.RemovePlayerFromGame(GameId, UserId);
            //dataManager.RemovePlayerFromGame(utr.GameId, utr.UserId);

            //return RedirectToAction("Index", new { id = utr.GameId });
            return RedirectToAction("Index", new { id = GameId });
        }
        public IActionResult MakeOwner(string UserId, int GameId)
        {

            dataManager.AddPlayerToOwner(GameId, UserId);

            return RedirectToAction("Index", new { id = GameId });
        }

        public IActionResult MakePeasant(string UserId, int GameId)
        {

            dataManager.RemoveOwner(GameId, UserId);

            return RedirectToAction("Index", new { id = GameId });
        }

        //public IActionResult UpdateGame (GameDetailsVM gdv)
        //{
        //    dataManager.UpdateGame(gdv);
        //    return RedirectToAction("Index", new { id = gdv.Id });
        //}

        public IActionResult ChangePublic(int gameId, bool boolToEdit)
        {
            dataManager.TogglePublic(gameId, boolToEdit);
            return RedirectToAction("Index", new { id = gameId });
        }
        public IActionResult ChangeActive(int gameId, bool boolToEdit)
        {
            dataManager.ToggleActive(gameId, boolToEdit);
            return RedirectToAction("Index", new { id = gameId });
        }
        public async Task<JsonResult> GetCurrentId()
        {
            var currentId = await dataManager.GetSingleUserId(User.Identity.Name);
            return Json(currentId);
        }
        public async Task<IActionResult> GetFieldId(string fieldName)
        {
            var fields = await dataManager.ListFields();
            var fieldId = fields.Where(o => o.Name == fieldName)
                .Select(o => o.Id)
                .SingleOrDefault();

            return Json(fieldId);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getFieldName (int id)
        {
            var result= await dataManager.GetOneFieldName(id);
            return Json(result);
        }
        public async Task<IActionResult> GetFieldCitys()
        {
            var result =await dataManager.GetFieldCitys();
            return Json(result);
        }
    }
}
