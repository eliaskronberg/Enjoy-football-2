using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }
        public int Id { get; set; }
        public List<Player> Players { get; set; }
        public string Name { get; set; }
    }
}
