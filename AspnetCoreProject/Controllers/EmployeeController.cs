using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreProject.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using AspnetCoreProject.Seeder;

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
            //var employee = _context.Employees.Where(o => o.Id == id).FirstOrDefault();

            //if (btn == "Delete")
            //{
            //    _context.Employees.Remove(employee);
            //    _context.SaveChanges();
            //    TempData["DeleteMessage"] = "Item is deleted successfuly";
            //    return RedirectToAction("Index");
            //}

            //return View();
            if (btn == "Delete")
            {
                using var transanction = _context.Database.BeginTransaction();
                try
                {
                    var empPict = _context.EmployeePictures.Where(o => o.EmpId == id).FirstOrDefault();
                    if(empPict!=null)
                    {
                        _context.EmployeePictures.Remove(empPict);
                        _context.SaveChanges();
                        
                    }
                    var emp = _context.Employees.Where(o => o.Id == id).FirstOrDefault();
                    _context.Employees.Remove(emp);
                    _context.SaveChanges();
                    transanction.Commit();

                    TempData["DeleteMessage"] = "Item is deleted successfuly";
                }
                catch (Exception)
                {

                    transanction.Rollback();
                }
            }
            return RedirectToAction("Index");
            
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

        [HttpPost]
        public async Task<IActionResult> EditPicture([FromForm]EmployeePicture model,List<IFormFile> files) 
        {
            if (ModelState.IsValid)
            {
                if (files.Count > 0)
                {
                    var FormFile = files[0];
                    MemoryStream ms = new MemoryStream();
                    await FormFile.CopyToAsync(ms);
                    model.ImageData = ms.ToArray();
                    model.ImageType = FormFile.ContentType;
                }
                model.Created = DateTime.Now;
                
                var emPict = _context.EmployeePictures.Where(o => o.EmpId == model.EmpId).FirstOrDefault();
                if (emPict == null)
                {
                 
                    _context.EmployeePictures.Add(model);
                    _context.SaveChanges();
                   
                }
                else
                {
                    _context.Entry(emPict).State = EntityState.Detached;
                    _context.EmployeePictures.Update(model);
                    _context.SaveChanges();

                }

                return RedirectToAction("ImageDetail", new { id = model.EmpId });

            }


            return View(model);
        }


        public IActionResult ImageDetail(int id)
        {
            ViewBag.ImageUrl = null;
            var empPict = this._context.EmployeePictures.Where(a => a.EmpId == id).FirstOrDefault();
            if (empPict != null)
            {
                string imgString = Convert.ToBase64String(empPict.ImageData);
                string imageDataURL = string.Format("data:{0};base64,{1}", empPict.ImageType, imgString);

                ViewBag.ImageUrl = imageDataURL;
            }

            var emp = this._context.Employees.Where(a => a.Id == id).FirstOrDefault();

            return View(emp);
        }

        public IActionResult Paging(int currentPageIndex)
        {
            if (currentPageIndex <= 0)
                currentPageIndex = 1;
            return View(GetEmployee(currentPageIndex));
        }

        private EmployeeView GetEmployee(int CurrentPage)
        {
            int nRows = 10;
            EmployeeView vw = new EmployeeView();
            vw.Employees = (from emp in _context.Employees select emp)
                .Skip((CurrentPage - 1) * nRows)
                .Take(nRows).ToList();
            vw.CurrentPageIndex = CurrentPage;
            double PageCount = (double)((decimal)_context.Employees.Count() / Convert.ToDecimal(nRows));
            vw.PageCount =(int)Math.Ceiling(PageCount);
            return vw;

        }

        public IActionResult Seed()
        {
            var SeedEmployee = new EmployeeSeeder(_context);
            SeedEmployee.Seed(100);
            return RedirectToAction("Index");
        }
        public IActionResult SortingPaging(EmployeeSortPagingView model)
        {
            if (model == null)
            {
                model = new EmployeeSortPagingView();
                model.CurrentIndexPage = 1;
                model.SortField = "Id";
                model.SortOrder = "ASC";
                model.CurrentSortField = "Id";
                model.CurrentSortOrder = "ASC";
            }
            else
            {
                if (model.CurrentIndexPage <= 0)
                    model.CurrentIndexPage = 1;
                if (string.IsNullOrEmpty(model.SortField))
                    model.SortField = "Id";
                if(model.SortField==model.CurrentSortField)
                {
                    if (model.CurrentSortOrder == model.SortOrder)
                        if (model.SortOrder == "ASC")
                            model.SortOrder = "DESC";
                        else
                            model.SortOrder = "ASC";

                }
                else
                {
                    if (string.IsNullOrEmpty(model.SortOrder))
                        model.SortOrder = "ASC";
                }
                
               
              
            }
            ViewBag.CurrentSortOrder = model.CurrentSortOrder;
            ViewBag.CurrentSortField = model.CurrentSortField;
            ViewBag.CurrentPageIndex = model.CurrentIndexPage;
            return View(GetEmployeesSortingPaging(model.CurrentIndexPage,model.CurrentSortField,model.CurrentSortOrder));
        }
        private EmployeeSortPagingView GetEmployeesSortingPaging(int currentPage, string field, string order)
        {
            int nRows = 10;
            EmployeeSortPagingView vw = new EmployeeSortPagingView();
            var propertyInfo = typeof(Employee).GetProperty(field);
            if (order == "ASC")
            {
                vw.Employees = (from emp in _context.Employees
                                select emp).ToList().OrderBy(emp => propertyInfo.GetValue(emp, null)).
                                Skip((currentPage - 1) * nRows).
                                Take(nRows).ToList();
            }
            else
            {
                vw.Employees = (from emp in _context.Employees
                                select emp).ToList().OrderByDescending(o => propertyInfo.GetValue(o, null)).
                              Skip((currentPage - 1) * nRows).Take(nRows).ToList();
            }
            double pageCount = (double)(decimal)(_context.Employees.Count() / Convert.ToDecimal(nRows));
            vw.PageCount = (int)Math.Ceiling(pageCount);
            vw.CurrentIndexPage = currentPage;
            vw.SortField = field;
            vw.SortOrder = order;

            return vw;


        }



    }
}
