using AspnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View(_userManager.Users);

        }

        public async Task<IActionResult> Details(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            return View(user);
        }
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            return View(user);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id,string btn)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (btn == "Delete")
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("List");
            }
            return View(user);
            
        }


    }
}
