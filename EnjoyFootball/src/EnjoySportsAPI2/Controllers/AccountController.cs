using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EnjoyFootball.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        SignInManager<IdentityUser> signInManager;
        DataManager datamanager;
        FootballContext context;
        UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, FootballContext context)
        {
            this.signInManager = signInManager;
            datamanager = new DataManager();
            this.context = context;
            this.userManager = userManager;
        }
        
        [HttpPost]
        public async Task<bool> Login(string userName, string password)
        {
            var result = await signInManager.PasswordSignInAsync(userName, password, false, false);
            //return RedirectToAction(nameof(HomeController.Index);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }



        [HttpPost("CreateUser/{player}")]
        public async Task<bool> CreateUser(string email, string password, string nickname, int age)
        {
            //Returnerar vy-modellen om något modellvärde är felaktigt
            
            await context.Database.EnsureCreatedAsync();
            IdentityUser newUser = new IdentityUser(email);
            var result = await userManager.CreateAsync(newUser, password);

            
            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            if (!result.Succeeded)
            {
                return false;
            }
            var newPlayer = new User();
            newPlayer.Id = datamanager.GetSingleUserId(email);
            newPlayer.Nickname = nickname;
            newPlayer.Age = age;

            datamanager.CreateNewPlayer(newPlayer);
            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress

            return true;
        }
    }
}
