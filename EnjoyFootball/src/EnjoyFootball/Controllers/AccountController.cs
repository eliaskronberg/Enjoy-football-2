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
    public class AccountController : Controller
    {
        SignInManager<IdentityUser> signInManager;
        // - Databaskopplingen
        FootballContext context;
        //IdentityDbContext identityContext;
        UserManager<IdentityUser> userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            FootballContext context, /*IdentityDbContext idcontext,*/ UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.context = context;
            this.userManager = userManager;
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

        public IActionResult CreateUser()
        {
            CreateUserVM newUser = new CreateUserVM();
            return View(newUser);
        }
        //Metoden är satt till async task för att metoden i sig är en async metod. Task säger att metoden är en asynkron operation.
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM model)
        {
            //Returnerar vy-modellen om något modellvärde är felaktigt
            if (!ModelState.IsValid)
                return View(model);

            await context.Database.EnsureCreatedAsync();
            IdentityUser newUser = new IdentityUser(model.EMail);
            var result = await userManager.CreateAsync(newUser, model.Password);
            
            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View(model);
            }
            //var userId = await userManager.GetUserIdAsync(newUser);
            //await userManager.AddToRoleAsync(newUser, "Default");
            //var category = context.UserCategory.Where(x => x.CategoryName == model.CategoryName).SingleOrDefault();

            //UserDetail userDetail = new UserDetail();
            //userDetail.Id = userId;

            //if (category != null)
            //{
            //    userDetail.SemesterId = category.Id;
            //    context.UserDetails.Add(userDetail);
            //    context.SaveChanges();
            //}
            //else
            //{
            //    UserCategory userCategory = new UserCategory();
            //    userCategory.CategoryName = model.CategoryName;
            //    context.UserCategory.Add(userCategory);
            //    context.SaveChanges();
            //    userDetail.SemesterId = userCategory.Id;
            //}
            ViewData["UserCreated"] = "1";
            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress

            return View(model);
        }
    }
}
