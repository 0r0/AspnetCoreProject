using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreProject.Models;
namespace AspnetCoreProject.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeProjectContext _context;

        public EmployeeController(EmployeeProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Employees);
        }

        public IActionResult Create()
        {
            return View(new Employee());
        }

    }
}
