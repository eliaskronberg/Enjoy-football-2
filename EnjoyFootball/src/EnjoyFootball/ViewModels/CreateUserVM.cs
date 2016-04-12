using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class CreateUserVM
    {
        [Required(ErrorMessage = "Skriv rätt för fan.")]
        [EmailAddress(ErrorMessage = "Ange giltig E-postadress, email@academic.se")]
        [Display(Name = "E-post")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Skriv rätt för fan.")]
        //[Range(6,20,ErrorMessage ="Felfelfelfelfelfel")]
        [Display(Name = "Password")]
        public string Password { get; set; }


    }
}
