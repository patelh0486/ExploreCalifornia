using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ExploreTexas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ExploreTexas.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly BlogDataContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(BlogDataContext db, IWebHostEnvironment hostEnvironment   )
        {
            this._db = db;
            this._hostEnvironment = hostEnvironment;
        }
          
       [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pagesize = 2;
            var totalposts = _db.Posts.Count();
            var totalpages = totalposts / pagesize;
            var previousPage = page - 1;
            var nextpage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextpage;
            ViewBag.HasNextPage = nextpage < totalpages;

            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Skip(pagesize * page)
                    .Take(pagesize)
                    .ToArray();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView(posts);

            return View(posts);

           /* var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();
            return View(posts);*/
        }

        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, long key)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Id == key);
            return View(post);
        }

        [Authorize]
        [HttpGet,Route("blogcreate")]
        public IActionResult BlogCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost,Route("blogcreate")]
        public async Task<IActionResult> BlogCreate( Post post)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

           
                post.Author = User.Identity.Name;
                post.Posted = DateTime.Now;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(post.ImageUpload.FileName);
                string extension = Path.GetExtension(post.ImageUpload.FileName);
                post.ImageName = filename = filename + extension;
                string path = Path.Combine(wwwRootPath + "/images/", filename);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                   await post.ImageUpload.CopyToAsync(fileStream);
                }
                _db.Posts.Add(post);
               await _db.SaveChangesAsync();
            
            

            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Id
            });
        }
    }
}
