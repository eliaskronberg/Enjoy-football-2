using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    public class TeamController : Controller
    {
        DataManager dataManager;
        public TeamController()
        {
            dataManager = new DataManager();
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int id)
        {
            var Json= await dataManager.getWebApi($"team/index/{id}");
            var result= JsonConvert.DeserializeObject<Team>(Json);
            return View(result);
        }
        public async Task<IActionResult> getAllTeams()
        {
            var recData = await dataManager.getWebApi("team/getallteams");
            var result = JsonConvert.DeserializeObject<List<Team>>(recData);
            var sendData = result.Select(o => o.Name).ToArray();
            return Json(sendData);
        }
        public async Task<IActionResult> Getteamsbyplayerid(string id)
        {
            var recData = await dataManager.getWebApi($"team/getTeamsByPlayerId/{id}");
            var result = JsonConvert.DeserializeObject<List<Team>>(recData);
            var sendData = result.Select(o => o.Name).ToArray();
            return Json(sendData);
        }
    }
}
