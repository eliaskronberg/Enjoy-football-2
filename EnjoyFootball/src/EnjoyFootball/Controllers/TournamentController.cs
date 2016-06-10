using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Newtonsoft.Json;
using EnjoyFootball.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    public class TournamentController : Controller
    {
        DataManager dataManager;
        public TournamentController()
        {
            dataManager = new DataManager();
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int id)
        {
            var result = await dataManager.getWebApi($"tournament/index/{id}");
            var tournament = JsonConvert.DeserializeObject<TournamentDetailsVM>(result);     
            return View(tournament);
        }

        public async Task<IActionResult> createTournament()
        {
            TournamentDetailsVM tournament = new TournamentDetailsVM();
            tournament.hostPlayerID = await dataManager.GetSingleUserId(User.Identity.Name);
            return View(tournament);
        }
       
        public async Task<IActionResult>alltournaments()
        {
            var Json = await dataManager.getWebApi("tournament/getalltournaments");
            var allTournaments = JsonConvert.DeserializeObject<List<TournamentDetailsVM>>(Json);
            return View(allTournaments);
        }

        public async Task<IActionResult> ManageMatch(int id)
        {
            var Json = await dataManager.getWebApi($"tournament/gettournamentmatchbyId/{id}");
            var match = JsonConvert.DeserializeObject<List<TournamentMatch>>(Json);
            return View(match);
        }

        public async Task<JsonResult> tournemantmatchbyid (int id)
        {
            var data = await dataManager.getWebApi($"tournament/gettournamentmatchbyId/{id}");
            var match = JsonConvert.DeserializeObject<TournamentMatch>(data);
            return Json(match);
        }
        public async Task<JsonResult> tournemantsbyteamid(int id)
        {
            var data = await dataManager.getWebApi($"tournament/gettournamentsbyteamid/{id}");
            var match = JsonConvert.DeserializeObject<List<TournamentDetailsVM>>(data);
            return Json(match);
        }


    }

}
