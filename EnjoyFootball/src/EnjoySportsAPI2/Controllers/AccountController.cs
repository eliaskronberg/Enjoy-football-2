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
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        SignInManager<IdentityUser> signInManager;
        DataManager datamanager;
        UserManager<IdentityUser> userManager;
        IdentityDbContext context;
        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, IdentityDbContext context)
        {
            this.context = context;
            this.signInManager = signInManager;
            datamanager = new DataManager();
            this.userManager = userManager;
        }
        
        //ANvänds ej
        [HttpGet("Login/{userSer}")]
        public async Task<SignInResult> Login(string userSer)
        {
            var email = userSer.Split(':')[0];
            var password = userSer.Split(':')[1];
            //"{\"Id\":null,\"Email\":\"markus_vall@hotmail.com\",\"Password\":\"4Ftonbladet!\",\"NickName\":null,\"Age\":0}"
            //"{\"Email\":\"markus_vall@hotmail.com\",\"Password\":\"4Ftonbladet!\"}"


            await context.Database.EnsureCreatedAsync();
            //var result = await signInManager.PasswordSignInAsync(Email, Password, false, false);
            var result = await signInManager.PasswordSignInAsync(email, password, false, false);
            //return RedirectToAction(nameof(HomeController.Index);
            //if (!result.Succeeded)
            //{
            //    return false;
            //}
            //return true;

            return result;
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
        [HttpGet("GetCurrentUser")]
        public string GetCurrentUser()
        {
            //var user = "markus_vall@hotmail.com";
            var result = datamanager.GetSingleUserId(User.Identity.Name);
            return result;
        }
    }
}
