using EnjoyFootball.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class TournamentDetailsVM
    {
        public int Id { get; set; }
        public string hostPlayerID { get; set; }
        public List<Team> Teams { get; set; }
        public List<TournamentMatch> Matches { get; set; }
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Antal lag i turneringen")]
        public int TournamentSize { get; set; }
        [Display(Name = "Beskrivning")]
        [Required]
        public string Description { get; set; }
    }
}
