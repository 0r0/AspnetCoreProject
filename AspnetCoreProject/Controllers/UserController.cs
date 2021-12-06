using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.User = User.Identity.Name;

            return View();
        }
        [AllowAnonymous]
        public IActionResult Anonymous()
        {
            return View(); 
        }
            }
}
