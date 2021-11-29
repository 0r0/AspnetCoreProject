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
        [HttpPost]
        public IActionResult Create([FromForm] Employee model)
        {
            //if (ModelState.IsValid)
            //{
            //    //return View();
            //    return RedirectToAction("Home");
            //}
            model.Created = DateTime.Now;
            _context.Employees.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

    }
}
