using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class FieldVM
    {
        public int Id { get; set; }
        public string Coordinates { get; set; }
        public string Name { get; set; }
        public string Turf { get; set; }
        public int Capacity { get; set; }
        public bool Lighting { get; set; }
        public string Description { get; set; }
        public double Condition { get; set; }
        public int Votes { get; set; }
    }
}
