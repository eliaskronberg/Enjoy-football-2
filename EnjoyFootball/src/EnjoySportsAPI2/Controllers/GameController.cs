using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Route("api/[controller]")]
    public class GameController : ApiController
    {
        DataManager dataManager;
        SignInManager<IdentityUser> signInManager;
        public GameController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
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
        [HttpPost("CreateGame")]
        public int CreateGame([FromBody]Game Dto)
        {
            var gameId = dataManager.CreateGame(Dto, Dto.Owner);
            if (gameId > 0)
            {
                dataManager.AddPlayerToGame(Dto.Owner, gameId);
                return gameId;
            }
            else
            {
                return 0;
            }

        }
        [HttpGet("addplayer/{id}/{playerName}")]
        public void Addplayer(int id, string playerName)
        {
            dataManager.AddPlayerToGame(playerName, id);
        }
        [HttpGet("removeplayer/{playerIdToRemove}/{id}")]
        public void RemovePlayer(string playerIdToRemove, int id)
        {
            dataManager.RemovePlayerFromGame(id, playerIdToRemove);
        }
        [HttpGet("MakeOwner/{UserId}/{GameId}")]
        public void MakeOwner(string UserId, int GameId)
        {
            dataManager.AddPlayerToOwner(GameId, UserId);
        }
        [HttpGet("MakePeasant/{UserId}/{GameId}")]
        public void MakePeasant(string UserId, int GameId)
        {
            dataManager.RemoveOwner(GameId, UserId);
        }
        [HttpPut ("updategame")]
        public void UpdateGame ([FromBody]Game gdv)
        {
            dataManager.UpdateGame(gdv);
        }
        [HttpGet("ChangePublic/{gameId}/{boolToedit}")]
        public void ChangePublic(int gameId, bool boolToEdit)
        {
            dataManager.TogglePublic(gameId, boolToEdit);
        }
        [HttpGet("ChangeActive/{gameId}/{boolToedit}")]
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
        [HttpGet("GetFieldId/{fieldName}")]
        public int GetFieldId(string fieldName)
        {
            var fields = dataManager.ListFields();
            var fieldId = fields.Where(o => o.Name == fieldName)
                .Select(o => o.Id)
                .SingleOrDefault();

            return fieldId;
        }
    }
}
