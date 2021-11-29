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
            return View();
           // new Employee()
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
        public IActionResult Details(int id)
        {
            var employee = _context.Employees.Where(o => o.Id == id).FirstOrDefault();
            return View(employee);
        }


        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Where(o => o.Id == id).FirstOrDefault();

            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit([FromForm] Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var employee=_context.Employees.Where(o => o.Id == id).FirstOrDefault();
            
            return View(employee);
        }
        [HttpPost]
        public IActionResult Delete(int id,string btn)
        {
            var employee = _context.Employees.Where(o => o.Id == id).FirstOrDefault();

            if (btn == "Delete")
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                TempData["DeleteMessage"] = "Item is deleted successfuly";
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult EditPicture(int id)
        {
            ViewBag.ImageUrl = null;
            var empPict = _context.EmployeePictures.Where(o => o.EmpId == id).FirstOrDefault();
            if (empPict == null)
                empPict = new EmployeePicture { EmpId = id };
            else
            {
                string imgString = Convert.ToBase64String(empPict.ImageData);
                string imageDataUrl = string.Format("data:{0};base64,{1}", empPict.ImageType, imgString);
                ViewBag.ImageUrl = imageDataUrl;
            }
            return View(empPict);
        }

    }
}
