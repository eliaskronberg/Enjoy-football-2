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
        DataManager dataManager;
        public FieldController()
        {
            this.dataManager = new DataManager();
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var list = dataManager.ListFields();
            var fieldVmList = new List<FieldVM>();
            foreach (var field in list)
            {
                FieldVM tmp = new FieldVM();
                tmp.Capacity = field.Capacity;
                tmp.Condition = field.Condition;
                tmp.Coordinates = field.Coordinates;
                tmp.Description = field.Description;
                tmp.Lighting = field.Lighting;
                tmp.Id = field.Id;
                tmp.Name = field.Name;
                tmp.Turf = field.Turf;
                tmp.Votes = field.Votes;
                fieldVmList.Add(tmp);
            }
            return View(fieldVmList);
        }

        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateField(CreateFieldVM viewModel)
        {
            var result = await dataManager.CreateField(viewModel);
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
