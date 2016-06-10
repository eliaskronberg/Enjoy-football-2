using EnjoySportsAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class Tournament
    {
        public Tournament()
        {
            Teams = new List<Team>();
            Matches = new List<TournamentMatch>();
        }
        public string Description { get; set; }
        public int Id { get; set; }
        public string hostPlayerID { get; set; }
        public List<Team> Teams { get; set; }
        public List<TournamentMatch> Matches { get; set; }
        public string Name { get; set; }
        public int TournamentSize { get; set; }

    }
}
