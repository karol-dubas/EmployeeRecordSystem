﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.Models.Services;
using MVC_FirstApp.Models.ViewModels;

namespace MVC_FirstApp.Controllers
{
    [Authorize(Roles = "Pracownik, Administrator")]
    public class GroupController : Controller
    {
        private readonly GroupService _gs;

        public GroupController(GroupService groupService)
        {
            _gs = groupService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = _gs.GetAll();

            return View(vm);
        }
    }
}
