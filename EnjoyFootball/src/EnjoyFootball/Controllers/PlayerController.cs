using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EnjoyFootball.ViewModels;
using Microsoft.AspNet.Authorization;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Authorize]

    public class PlayerController : Controller
    {
        DataManager dataManager;
        UserManager<IdentityUser> usrmanager;
        public PlayerController(UserManager<IdentityUser> usr)
        {
            usrmanager = usr;
            dataManager = new DataManager();
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(string Id="")
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("index", "home");
            }
            var userId = await dataManager.GetSingleUserId(User.Identity.Name);
            var playerToShow = await dataManager.GetPlayerInfo(Id);
            PlayerInfoVM playerinfo = new PlayerInfoVM();
            playerinfo.player = playerToShow;
            if (userId == Id)
            {
                playerinfo.currentUser = true;
            }
            else
            {
                playerinfo.currentUser = false;
            }
            return View(playerinfo);
        }
        public async Task<IActionResult> GetOwnerNickNames(int id)
        {
            
            var game= await dataManager.getGameByID(id);
            var playersInGame = await dataManager.PlayerByGameId(id);
            var listOfOwnerNicknames = new List<string>();
            foreach (var item in playersInGame)
            {
                foreach (var owner in game.Owner.Split(';'))
                {
                   var player = await dataManager.GetPlayerInfo(owner);
                    listOfOwnerNicknames.Add(player.Nickname);
                }
            }
            return Json(listOfOwnerNicknames);
        }

        public async Task<IActionResult> GetGamesByPlayerId(string id)
        {
           var games= await dataManager.GetGameIdsByPlayerId(id);
            return Json(games);
        }
        public async Task<IActionResult> GetTeamsByPlayerId(string id)
        {
            var teams = await dataManager.getWebApi($"player/GetTeamsByPlayerId/{id}");
            var result = JsonConvert.DeserializeObject<List<Team>>(teams);
            return Json(result);
        }

        public async Task<IActionResult>getPlayerIdByName(string name)
        {
            var personId= Json(await dataManager.GetSingleUserId(name));
            return RedirectToAction("index", "player", new { id = personId.Value });
        }

        public async Task<IActionResult>getPlayerNameById(string id)
        {
            var personName = await dataManager.GetPlayerInfo(id);
            return Json(personName.Nickname);
        }
    }
}
