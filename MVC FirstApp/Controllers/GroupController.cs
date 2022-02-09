using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FirstApp.ViewModels;
using MVC_FirstApp.Services;
using MVC_FirstApp.Constants;

namespace MVC_FirstApp.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Worker}, {Roles.Admin}")]
        public IActionResult Index()
        {
            var vm = _groupService.GetAll();

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult EditHours(long groupId)
        {
            var vm = _groupService.GetGroupUsers(groupId);

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult EditHours(UserHoursViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var error = _groupService.UpdateTime(data);

            if (error)
            {
                ModelState.AddModelError("", "Jeden z użytkowników miałby wartość przepracowanych godzin mniejszą od 0");

                return View(data);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddMoney()
        {
            _groupService.AddMoney();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddGroup(string name)
        {
            _groupService.AddGroup(name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RenameGroup(long id, string newName)
        {
            _groupService.RenameGroup(id, newName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteGroup(long id)
        {
            _groupService.DeleteGroup(id);
            return RedirectToAction("Index");
        }
    }
}
