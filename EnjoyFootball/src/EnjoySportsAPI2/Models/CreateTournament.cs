using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoySportsAPI2.Models
{
    public class CreateTournament
    {
        public int Id { get; set; }
        public string hostPlayerID { get; set; }
        public string Name { get; set; }
        public List<string> TeamNames { get; set; }
        public int TournamentSizeTeams { get; set; }
        public string Description { get; set; }
    }
}
