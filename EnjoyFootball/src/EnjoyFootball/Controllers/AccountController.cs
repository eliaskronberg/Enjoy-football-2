using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EnjoyFootball.Models;
using EnjoyFootball.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        SignInManager<IdentityUser> signInManager;
        // - Databaskopplingen
        FootballContext context;
        //IdentityDbContext identityContext;
        UserManager<IdentityUser> userManager;
        DataManager datamanager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            FootballContext context, /*IdentityDbContext idcontext,*/ UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.context = context;
            this.userManager = userManager;
            datamanager = new DataManager();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginvm);
            }


            // Inbyggd metod för att sköta dekrypteringar och säkerhet, de sista två är isPersistent (loginfail !=> rensa inloggningsuppgifter) och lockOutOnFailure

            //await context.Database.EnsureCreatedAsync();
            var result = await signInManager.PasswordSignInAsync(loginvm.EMail, loginvm.Password, false, false);


            //return RedirectToAction(nameof(HomeController.Index);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("errormessage", "Inloggning misslyckades.");
                loginvm.Password = "";
                return View(loginvm);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [AllowAnonymous]
        public IActionResult CreateUser()
        {
        //    CreateUserVM newUser = new CreateUserVM();
            return View();
        }
        //Metoden är satt till async task för att metoden i sig är en async metod. Task säger att metoden är en asynkron operation.
        [HttpPost]
        public IActionResult CreateUser(CreateUserVM model)
        {
            //Returnerar vy-modellen om något modellvärde är felaktigt
            if (!ModelState.IsValid)
                return View(model);

            // Hanteras i api:t
            //await context.Database.EnsureCreatedAsync();
            //IdentityUser newUser = new IdentityUser(model.EMail);
            //var result = await userManager.CreateAsync(newUser, model.Password);
            User user = new User();
            user.Email = model.EMail;
            user.NickName = model.Nickname;
            user.Password = model.Password;
            user.Age = model.Age;
            datamanager.CreateNewPlayer(user);
            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            
            ViewData["UserCreated"] = "1";
            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress

            return RedirectToAction(nameof(Login));
        }
    }
}
