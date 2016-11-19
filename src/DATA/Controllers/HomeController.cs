using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using DATA.Helper;

namespace DATA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Graphs()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
