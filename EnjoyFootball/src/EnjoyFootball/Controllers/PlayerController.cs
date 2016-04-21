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
        public async Task<IActionResult> Index(string Id = "")
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
    }
}
