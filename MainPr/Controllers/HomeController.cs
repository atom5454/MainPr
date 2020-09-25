using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;

namespace MainPr.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        //[HttpGet]
        //public async Task<string> GetCurrentUserId()
        //{
        //    User user = await _userManager.GetUserAsync(HttpContext.User);
        //    return ViewBag.login = user?.Login;
        //}

        public IActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    User user = await _userManager.GetUserAsync(HttpContext.User);
            //    ViewBag.login = user?.Login;
            //    return View();
            //}
            //else
            //{
            //    return View();
            //}
            //GetCurrentUserId();
            return View();
        }
        public IActionResult Privacy()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    User user = await _userManager.GetUserAsync(HttpContext.User);
            //    ViewBag.login = user?.Login;
            //    return View();
            //}
            //else
            //{
            //    return View();
            //}
            //GetCurrentUserId();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
