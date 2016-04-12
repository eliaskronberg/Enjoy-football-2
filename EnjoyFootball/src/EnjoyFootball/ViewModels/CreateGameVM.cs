using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class CreateGameVM
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartTime { get; set; }
        public string Field { get; set; }
        public bool IsActive { get; set; }
        public int MaxSlots { get; set; }
        
    }
}
