using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class Game
    {
        public Game()
        {
            OwnerNickNames = new List<string>();
        }
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartTime { get; set; }
        public int Field { get; set; }
        public bool IsActive { get; set; }
        public int MaxSlots { get; set; }
        public List<string> OwnerNickNames { get; set; }
    }
}
