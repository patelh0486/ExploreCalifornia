using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreTexas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreTexas.Controllers
{
    [Route("support")]
    public class SupportController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,Route("hbo")]
        public IActionResult hbo()
        {
            return View();
        }

        /*[Route("")]
        public IActionResult Post()
        {
            //var post = _db.Posts.FirstOrDefault(x => x.Key == key);
            //return View(post);
        }*/

        [HttpPost, Route("hbo")]
        public IActionResult hbo(Supportformdata supportdata)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            return View(supportdata);
            /*return RedirectToAction("Post", "Blog",null);*/
        }
    }
}
