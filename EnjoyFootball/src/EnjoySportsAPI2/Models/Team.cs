using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class Team
    {
        public int Id { get; set; }
        public List<Player> Players { get; set; }
    }
}
