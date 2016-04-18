using EnjoyFootball.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class GameOverviewVM
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartTime { get; set; }
        public string FieldName { get; set; }
        public List<Player> PlayerList { get; set; }
        public bool IsActive { get; set; }
        public int MaxSlots { get; set; }
        public string Coordinates { get; set; }
    }
}
