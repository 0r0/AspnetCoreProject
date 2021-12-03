﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SessionView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SessionView(int a, int b, string c)
        {
            HttpContext.Session.SetInt32("a", a);
            HttpContext.Session.SetInt32("b", b);
            HttpContext.Session.SetInt32("ab", a + b);
            HttpContext.Session.SetString("c", c);
            TempData["a"] = a;
            TempData["b"] = b;
            TempData["ab"] = a + b;
            TempData["c"] = c;

            return View();
        }
        

    }
}
