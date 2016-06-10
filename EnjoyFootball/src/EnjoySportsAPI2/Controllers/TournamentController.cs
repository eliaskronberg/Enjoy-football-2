using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;
using EnjoySportsAPI2.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoySportsAPI2.Controllers
{
        [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        DataManager dataManager;
        public TournamentController()
        {
            dataManager = new DataManager();
        }
        // GET: /<controller>/
        [HttpGet("Index/{id}")]
        public Tournament Index(int id)
        {
            var tournament = dataManager.GetTournamentById(id);
            tournament.Teams = dataManager.getTournamentTeamssByTournamentId(id);
            tournament.Matches = dataManager.getTournamentGamesByTourId(id);
            return tournament;
        }


        [HttpPost("CreateTournament")]
        public int createTournament(CreateTournament tournament)
        {
            Tournament tourToCreate = new Tournament();
            var allTeams = dataManager.getAllTeams();
           
            foreach (var team in allTeams)
            {
                if (tournament.TeamNames.Contains(team.Name))
                {
                    tourToCreate.Teams.Add(team);
                }
            }
            tourToCreate.Name= tournament.Name;
            tourToCreate.hostPlayerID= tournament.hostPlayerID;

            var tournamentId= dataManager.CreateTournament(tourToCreate.hostPlayerID, tourToCreate.Name, tourToCreate.Teams[0], tournament.Description, tournament.TournamentSizeTeams);

            for (int i = 0; i < tournament.TournamentSizeTeams-1; i++)
            {
                TournamentMatch tmpMatch = new TournamentMatch();
                tmpMatch.tournamentId = tournamentId;
                tourToCreate.Matches.Add(tmpMatch);
            }

            double numberOfRounds = System.Math.Sqrt(tournament.TournamentSizeTeams);
            var RealRounds = Math.Ceiling(numberOfRounds);
            var filledRound = 1;
            List<TournamentMatch> matchesToSet = new List<TournamentMatch>();
            double matchesLeft = tourToCreate.Matches.Count / 2.0;
            matchesToSet = tourToCreate.Matches.GetRange(0, Convert.ToInt32(Math.Ceiling(matchesLeft)));
           
            int matchCount = 0;
           
            while (filledRound != RealRounds+1)
            {
                
                    setTournamentGameRound(filledRound, matchesToSet);
                    matchCount+=matchesToSet.Count;
                
                    filledRound++;
                matchesLeft= matchesLeft / 2;
                if (matchesLeft > 0.5)
                {
                    matchesToSet = tourToCreate.Matches.GetRange(matchCount, Convert.ToInt32(Math.Ceiling(matchesLeft)));
                }
            }

            int nofrg = tourToCreate.Matches.Where(o => o.TournamentRound == 1).Count();
            int j = 0;
            for (int i = 0; i < tourToCreate.Teams.Count; i++)
            {
                    tourToCreate.Matches[j].teamOne = tourToCreate.Teams[i];
                if (tourToCreate.Teams.Count > i+1)
                {
                    tourToCreate.Matches[j].teamTwo = tourToCreate.Teams[i + 1];
                }
                i++;
                j++;
            }

            foreach (var match in tourToCreate.Matches)
            {
                if(match.teamOne==null||match.teamOne.Name=="")
                {
                    match.teamOne = new Team();                 
                    match.teamOne.Name = "Unknown";   
                }
                if (match.teamTwo == null || match.teamTwo.Name == "")
                {
                    match.teamTwo = new Team();
                    match.teamTwo.Name = "Unknown";
                }
                    dataManager.createTournamentGame(match);
            }

            if (tournamentId > 0)
            {
                foreach (var team in tourToCreate.Teams)
                {
                    dataManager.AddTeamToTournament(team.Id, tournamentId);
                }
                return tournamentId;
            }
            else
            {
                return 0;
            }
        }

        [HttpPost("AddTeamToTournament")]
        public void AddTeamToTournament(int tournamentId, string teamName)
        {
            var myTeam=dataManager.getAllTeams().Where(o => o.Name == teamName).SingleOrDefault();
            dataManager.AddTeamToTournament(myTeam.Id, tournamentId);

            var allTourGames = dataManager.getTournamentGamesByTourId(tournamentId);

            var firstEmptySpotGame = allTourGames.Where(o => o.teamOne.Name == null || o.teamTwo.Name == null).Where(o => o.TournamentRound==1).FirstOrDefault();

            setTeamForNewRound(firstEmptySpotGame.Id, myTeam);
        }

        [HttpGet("getlasttournament")]
        public Tournament GetLastTournament()
        {
            return dataManager.GetAllTournaments().Last();
        }

        private List<TournamentMatch> setTournamentGameRound(int filledRounds,  List<TournamentMatch> matchList)
        {
            foreach (var match in matchList)
            {
                match.TournamentRound = filledRounds;
            }
            return matchList;
        }

        [HttpGet ("getAllTournaments")]
        public List<Tournament> getAllTournaments()
        {
            var allTournaments= dataManager.GetAllTournaments();
            foreach (var item in allTournaments)
            {
                item.Teams = dataManager.getTournamentTeamssByTournamentId(item.Id);
            }
            return allTournaments;
        }
        [HttpGet("gettournamentsbyteamid/{id}")]
        public List<Tournament> Gettournamentsbyteamid(int id)
        {
           var hey= dataManager.getTournamentByTeamId(id);
            return hey;
        }
        [HttpGet("gettournamentmatchbyId/{id}")]
        public TournamentMatch GetTournamentMatchById(int id)
        {
           return dataManager.getTournamentMatchByid(id);
        }
        [HttpPost("ManageMatch")]
        public void ManageTournamentMatch(UpdateGame matchresult)
        {
            string dataBaseResult = matchresult.GoalsTeamOne + "," + matchresult.GoalsTeamTwo;
            bool isTeamOneWinner = false;
            bool isPlayed;
            if (matchresult.GoalsTeamOne > matchresult.GoalsTeamTwo)
                isTeamOneWinner = true;
            else
                isTeamOneWinner = false;

            dataManager.updateTourMatch(true, isTeamOneWinner, matchresult.TournamentGameId, dataBaseResult);

            var allTourGames= dataManager.getTournamentGamesByTourId(matchresult.TournamentId);

            var firstEmptySpotGame = allTourGames.Where(o => o.teamOne.Name == null || o.teamTwo.Name == null).Where(o=>o.TournamentRound-1== dataManager.getTournamentMatchByid(matchresult.TournamentGameId).TournamentRound).FirstOrDefault();

            var tourMatch = dataManager.getTournamentMatchByid(matchresult.TournamentGameId);

            if (firstEmptySpotGame != null)
            {
                if (isTeamOneWinner)
                {
                    setTeamForNewRound(firstEmptySpotGame.Id, tourMatch.teamOne);
                }
                else
                {
                    setTeamForNewRound(firstEmptySpotGame.Id, tourMatch.teamTwo);
                }
            }
        }

        [HttpPost("setTeamForNewRound")]
        public void setTeamForNewRound(int matchId, Team team)
        {
            dataManager.SetTeamsInTourGame(matchId, team);
        }

    }
}
