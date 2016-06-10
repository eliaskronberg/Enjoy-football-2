using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public int OpenSlots{ get; set; }
        public DateTime TimeOfMatch { get; set; }

    }
}
