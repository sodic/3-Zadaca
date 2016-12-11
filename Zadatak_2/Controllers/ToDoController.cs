using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zadatak_1;

namespace Zadatak_2.Controllers
{
    public class ToDoController : Controller
    {
        
        private readonly IToDoRepository _repository;

        public ToDoController(IToDoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}