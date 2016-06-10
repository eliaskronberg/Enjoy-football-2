using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class CreateFieldVM
    {
        [Display(Name="Location")]
        public string Coordinates { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Turf { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public bool Lighting { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

    }
}
