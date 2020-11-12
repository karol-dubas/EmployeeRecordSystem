using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_FirstApp.Models;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserEditService _us;
        private UserManager<ApplicationUser> _um;

        public HomeController(UserEditService userService, UserManager<ApplicationUser> userManager)
        {
            _us = userService;
            _um = userManager;
        }

        public IActionResult Index()
        {
            var userId = _um.GetUserId(User);
            var vm = _us.GetUserDetails(userId);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
