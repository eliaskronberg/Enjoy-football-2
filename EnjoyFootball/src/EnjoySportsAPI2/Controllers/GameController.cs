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
    public class GameController : Controller
    {
        DataManager dataManager;
        public GameController()
        {
            dataManager = new DataManager();
        }

        // GET: /<controller>/
        [HttpGet("Index/{id}")]
        public Game Index(int id)
        {
            Game tempGame = dataManager.getGameByID(id);
            return tempGame;
        }
        [HttpGet("GetPlayersByGameId/{id}")]
        public List<Player> GetPlayersByGameId(int id)
        {
            List<Player> playerlist = dataManager.PlayerByGameId(id);
            return playerlist;
        }
       
        public string[] ListFieldName()
        {
            var model = dataManager.GetAllFieldNames();
            return model;
        }
        [HttpPost]
        public bool CreateGame(Game createGameVm, string userId)
        {
            var gameId = dataManager.CreateGame(createGameVm, userId);
            if (gameId > 0)
            {
                dataManager.AddPlayerToGame(User.Identity.Name, gameId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Addplayer(int id, string playerName)
        {
            dataManager.AddPlayerToGame(playerName, id);
        }

        public void RemovePlayer(string UserId, int GameId)
        {
            dataManager.RemovePlayerFromGame(GameId, UserId);
        }
        public void MakeOwner(string UserId, int GameId)
        {
            dataManager.AddPlayerToOwner(GameId, UserId);
        }

        public void MakePeasant(string UserId, int GameId)
        {
            dataManager.RemoveOwner(GameId, UserId);
        }

        public void UpdateGame (Game gdv)
        {
            dataManager.UpdateGame(gdv);
        }

        public void ChangePublic(int gameId, bool boolToEdit)
        {
            dataManager.TogglePublic(gameId, boolToEdit);
        }
        public void ChangeActive(int gameId, bool boolToEdit)
        {
            dataManager.ToggleActive(gameId, boolToEdit);
        }
        [HttpGet("GetFieldName/{id}")]
        public string GetFieldName(int id)
        {
            var model = dataManager.GetFieldById(id).Name;
            return model;
        }
    }
}
