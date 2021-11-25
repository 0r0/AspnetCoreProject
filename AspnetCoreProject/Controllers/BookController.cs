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

        public IActionResult BookDetail([FromRoute] int id)
        {
            Book o = new Book();
            o.Id = id;
            o.Title = string.Format("Book title {0}",id);
            o.Author = string.Format("Author {0}",id);
            o.Email = string.Format("dotnet{0}@email.com", id);
            o.BookCategory = "Programming";
            o.Description = string.Format("Description for title {0}",id);

            return View("detail",o);
        }
        

        public IActionResult ShowDetail()
        {
            Book o = new Book();
            o.Id = 1;
            o.Title = "Essential Asp.Net Core 5";
            o.Author = "Mr .Net";
            o.Email = "DotNet@gmail.com";
            o.BookCategory = "programming";
            o.Description = "this book  is designed for .Net Developers";

            return View(o);

        }

        public IActionResult ShowBook(int id)
        {
            return RedirectToAction("BookDetail",new { id=id});
        }
        public IActionResult ViewComponentDemo()
        {
            return View();
        }

        public IActionResult LayoutDemo()
        {

            return View();
        }


        public IActionResult RoutingDemo()
        {
            return View();
        }
    }
}
