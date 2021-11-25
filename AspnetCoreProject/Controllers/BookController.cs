using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreProject.Models;
namespace AspnetCoreProject.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowBooks()
        {
            return View();
        }
        public IActionResult Demo()
        {
            return View("Index");
        }

        public IActionResult PartialDemo()
        {
            return View();
        }

        public IActionResult ShowDetail()
        {
            Book o = new Book();
            o.Id = 1;
            o.Title = "Essential Asp.Net Core 5";
            o.Author = "Mr .Net";
            o.Email = "DotNet@gmail.com";
            o.BookCateory = "programming";
            o.Description = "this book  is designed for .Net Developers";

            return View(o);

        }
        public IActionResult ViewComponentDemo()
        {
            return View();
        }

        public IActionResult LayoutDemo()
        {

            return View();
        }
    }
}
