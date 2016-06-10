using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoySportsAPI2.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        DataManager datamanager;

        public TeamController()
        {
            datamanager = new DataManager();
        }
        // GET: /<controller>/
        [HttpGet("index/{id}")]
        public Team Index(int id)
        {
            var teamMemebers=datamanager.getTeamPlayersByTeamId(id);
            var theTeam= datamanager.GetTeamById(id);
            theTeam.Players = teamMemebers;
            return theTeam;
        }
        [HttpGet("getAllTeams")]
        public List<Team> getAllTeams()
        {
            return datamanager.getAllTeams();
        }

        [HttpGet("getTeamsByPlayerId/{id}")]
        public List<Team> getTeamsByPlayerId(string id)
        {
            var playerId=datamanager.GetSingleUserId(id);
            return datamanager.GetTeamsByPlayerId(playerId);
        }
    }
}
