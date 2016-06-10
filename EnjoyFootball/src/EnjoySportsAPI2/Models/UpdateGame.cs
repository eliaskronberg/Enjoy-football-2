namespace EnjoyFootball.Models
{
    public class UpdateGame
    {
        public int GoalsTeamOne { get; set; }
        public int GoalsTeamTwo { get; set; }
        public int TournamentGameId { get; set; }
        public int TournamentId { get; set; }
    }
}