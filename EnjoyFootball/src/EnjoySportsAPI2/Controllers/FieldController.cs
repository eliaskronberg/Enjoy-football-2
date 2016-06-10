using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
        [Route("api/[controller]")]
    public class FieldController : Controller
    {
        DataManager dataManager;
        public FieldController()
        {
            this.dataManager = new DataManager();
        }
        // GET: /<controller>/
        [HttpGet("Index")]
        public List<Field> Index()
        {
            var list = dataManager.ListFields();
            return list;
            //var fieldList = new List<Field>();
            //foreach (var field in list)
            //{
            //    Field tmp = new Field();
            //    tmp.Capacity = field.Capacity;
            //    tmp.Condition = field.Condition;
            //    tmp.Coordinates = field.Coordinates;
            //    tmp.Description = field.Description;
            //    tmp.Lighting = field.Lighting;
            //    tmp.Id = field.Id;
            //    tmp.Name = field.Name;
            //    tmp.Turf = field.Turf;
            //    tmp.Votes = field.Votes;
            //    fieldList.Add(tmp);
            //}
            //return fieldList;
        }

        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost("CreateField")]
        public bool CreateField([FromBody]Field viewModel)
        {
            var result = dataManager.CreateField(viewModel);
            if (result)
                return true;
            else
            {
                return false;
            }
        }
    }
}
