using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zadatak_1;
using Zadatak_2.Models;

namespace Zadatak_2.Controllers
{
    public class ToDoController : Controller
    {
        
        private readonly IToDoRepository _repository;
        private readonly UserManager<ApplicationUser> _users;

        public ToDoController(IToDoRepository repository,UserManager<ApplicationUser> users)
        {
            _repository = repository;
            _users = users;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _users.GetUserAsync(HttpContext.User);
            var items = _repository.GetActive(Guid.Parse(user.Id));
            return View(items);
        }
        [Authorize]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _users.GetUserAsync(HttpContext.User);
                ToDoItem todoItem = new ToDoItem(item.Text, Guid.Parse(currentUser.Id));
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [Authorize]
        public async Task<IActionResult> Completed()
        {
            ApplicationUser user = await _users.GetUserAsync(HttpContext.User);
            var items = _repository.GetCompleted(Guid.Parse(user.Id));
            return View(items);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            _repository.MarkAsCompleted(id, Guid.Parse((await _users.GetUserAsync(HttpContext.User)).Id));
            return RedirectToAction("Index");
        }
    }
}