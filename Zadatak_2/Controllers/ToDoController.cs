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

        //public async Task<IActionResult> add(String text)
        //{
        //    ApplicationUser user = await _users.GetUserAsync(HttpContext.User);
        //    var item = new ToDoItem(text, Guid.Parse(user.Id));
        //}
        public async Task<IActionResult> AddNewToDo(string text)
        {
            ApplicationUser user = await _users.GetUserAsync(HttpContext.User);
            var item=new ToDoItem(text,Guid.Parse(user.Id));
            _repository.Add(item);
            return RedirectToAction("Index");
        }

        public IActionResult SeeCompletedToDos()
        {
            throw new NotImplementedException();
        }

        public IActionResult MarkAsCompleted()
        {
            throw new NotImplementedException();
        }
    }
}