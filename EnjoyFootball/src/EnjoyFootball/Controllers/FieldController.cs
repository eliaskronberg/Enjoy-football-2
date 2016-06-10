using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.ViewModels;
using EnjoyFootball.Models;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    [Authorize]
    public class FieldController : Controller
    {
        DataManager dataManager;
        public FieldController()
        {
            this.dataManager = new DataManager();
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var list = await dataManager.ListFields();
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
                tmp.City = field.City;
                fieldVmList.Add(tmp);
            }
            return View(fieldVmList);
        }
        public async Task<IActionResult> GetFieldById(int id)
        {
            var allfields = await dataManager.ListFields();
            var theField=allfields.Where(o => o.Id == id).SingleOrDefault();
            return Json(theField);
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
