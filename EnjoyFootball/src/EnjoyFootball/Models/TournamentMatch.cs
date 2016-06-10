using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class TournamentMatch
    {
        public Team teamOne { get; set; }
        public Team teamTwo { get; set; }
        public int tournamentId { get; set; }
        public bool isTeamOneWinner { get; set; }
        public bool isPlayed { get; set; }
        public int Id { get; set; }
        public int TournamentRound { get; set; }
        public string Result { get; set; }
    }
}
