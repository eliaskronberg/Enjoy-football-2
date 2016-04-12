using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EnjoyFootball.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnjoyFootball.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            List<Match> listOfMatches = new List<Match>();

            listOfMatches.Add(new Match { Id = 1, Owner = "Martin(alltid)", Location = "Vasaparken", OpenSlots = 10, TimeOfMatch = DateTime.Now });
            listOfMatches.Add(new Match { Id = 2, Owner = "Martin(alltid)", Location = "GrimstaIp", OpenSlots = 7, TimeOfMatch = DateTime.Now });
            listOfMatches.Add(new Match { Id = 3, Owner = "Martin(alltid)", Location = "GrimstaBeach", OpenSlots = 1, TimeOfMatch = DateTime.Now });
            listOfMatches.Add(new Match { Id = 4, Owner = "Martin(alltid)", Location = "Husby", OpenSlots = 3, TimeOfMatch = DateTime.Now });
            return View(listOfMatches);
            //return PartialView("IndexPartial", listOfMatches);
        }
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
