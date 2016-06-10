using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("Index/{id}")]

        public Player Index(string Id = "")
        {
            if (string.IsNullOrEmpty(Id))
            {
                return new Player();
            }
            var playerToShow = dataManager.GetPlayerInfo(Id);
            return playerToShow;
        }
        [HttpGet("GetPlayerIdByUsername/{userName}")]
        public string GetPlayerIdByUsername(string userName)
        {
            var playerId = dataManager.GetSingleUserId(userName);
            return playerId;
        }

        [HttpGet ("GetGamesByPlayerId/{id}")]
        public List<int> GetGamesByPlayerId (string id)
        {
            var gameIds=dataManager.getGamesByPlayerId(id);
            return gameIds;
        }
        [HttpGet("GetTeamsByPlayerId/{id}")]
        public List<Team> GetTeamByPlayerId(string id)
        {
            var Teams = dataManager.GetTeamsByPlayerId(id);
            return Teams;
        }
    }
}
