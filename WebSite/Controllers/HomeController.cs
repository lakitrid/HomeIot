using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Configuration;

namespace WebSite.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        [FromServices]
        public IConfiguration Configuration { get; set; }

        public IActionResult Index()
        {
            ViewBag.Version = Configuration.Get<string>("site:version");

            return View();
        }
    }
}
