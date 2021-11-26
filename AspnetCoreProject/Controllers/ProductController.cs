using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspnetCoreProject.Models;

using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace AspnetCoreProject.Controllers
{
    public class ProductController : Controller
    {
        private IWebHostEnvironment _environment;
        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] Product model)
        {
            if (ModelState.IsValid)
            {
                return View("Details", model);
            }
            return View();
        }
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] Product obj, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (files.Count > 0)
                {
                    var formFile = files[0];
                    var filePath = this._environment.WebRootPath + "/img/" + formFile.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    ViewBag.ImageUrl = "/img/" + formFile.FileName;
                }

                return View("ImageDetail", obj);
            }
            return View();
        }
        public IActionResult ShowProducts()
        {
            List<Product> list = new List<Product>();
            for(int i = 0; i < 10; i++)
            {
                Product o = new Product
                {
                    Id = i + 1,
                    Name = string.Format("name {0}" , (i + 1)),
                    Price = (float)(1.5 + 2.5 * i),
                    Quantity=(10+i*3)

                };
                list.Add(o);
            }
            return View(list);
        }
        public IActionResult ViewReport()
        {
            ViewBag.Data1 = new int[] { 10, 80, 90, 40, 33, 70, 20,50,60,80 };
            ViewBag.Data2 = new int[] { 50, 30, 50, 70, 80, 40, 55, 80, 32 ,23};
            return View();
        }
        
    }
}
