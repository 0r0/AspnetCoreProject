using AspnetCoreProject.Models;
using AspnetCoreProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        //private IMailService _mailService;
        public AccountController(UserManager<AppUser> userManager,
           // IMailService mailService,
            SignInManager<AppUser> signInManager
            ,RoleManager<IdentityRole> roleManager)
        
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            //_mailService = mailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserView model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email

                };
                //check identity
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                    //add role
                    //IdentityResult result2 = await _userManager.AddToRoleAsync(user, "Member");
                    //if (result2.Succeeded)
                    //{
                    //    return RedirectToAction("Index");
                    //}
                }
            }
            return View(model);
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginView());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginView model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/account");
                    }

                }
                ModelState.AddModelError(nameof(LoginView.UserName), "Invalid User/Password or account is not activated yet");

            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return View();
        }
    }
}
