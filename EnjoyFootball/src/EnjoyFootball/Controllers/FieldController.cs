using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.ViewModels;
using EnjoyFootball.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    public class FieldController : Controller
    {
        FootballContext context;
        DataManager dataManager;
        public FieldController(FootballContext context)
        {
            this.context = context;
            this.dataManager = new DataManager(context);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var list = dataManager.ListFields();
            return View(list);
        }

        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateField(CreateFieldVM viewModel)
        {
            var result = dataManager.CreateField(viewModel);
            if (result)
                return RedirectToAction(nameof(Index));
            else
            {
                ViewData["FieldIsInvalid"] = "1";
                return View(viewModel);
            }
        }
    }
}
