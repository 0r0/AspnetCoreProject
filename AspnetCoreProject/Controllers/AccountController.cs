using AspnetCoreProject.Models;
using AspnetCoreProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspnetCoreProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IMailService _mailService;
        private IRecaptchaService _recaptchaService;
        public AccountController(UserManager<AppUser> userManager,
            IMailService mailService,
            SignInManager<AppUser> signInManager
            ,RoleManager<IdentityRole> roleManager,
            IRecaptchaService recaptchaService)
        
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailService = mailService;
            _recaptchaService = recaptchaService;
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

        public async Task<IActionResult> GenerateRoles()
        {
            string[] roleNames = { "Admin", "Manager", "Member" };
            IdentityResult roleResult;
            foreach(var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                }
            }
            return View();
        }

        public async Task<IActionResult> GenerateUsers()
        {
            AppUser userAv1 = await _userManager.FindByNameAsync("manager");
            if(userAv1==null)
            {
                AppUser user1 = new AppUser
                {
                    UserName = "manager",
                    FullName = "Manager",
                    Email = "manager@email.com"
                };
                IdentityResult result = await _userManager.CreateAsync(user1, "Pass@word1");
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user1, "Manager");

                }
            }
            AppUser userAv2 = await _userManager.FindByEmailAsync("admin@email.com");
            if (userAv2 == null)
            {
                //define
                AppUser user2 = new AppUser
                {
                    UserName = "admin",
                    FullName = "Admin",
                    Email = "admin@email.com"
                };
                //create user
                IdentityResult result = await _userManager.CreateAsync(user2, "Pass@word1");
                if (result.Succeeded)
                {

                    // add admin role
                    await _userManager.AddToRoleAsync(user2, "Admin");
                }
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail()
        {
            MailRequest req = new MailRequest();
            req.ToEmail = "Example@Example.com";
            req.Subject = "Test Email From ASP.Net Core 5 MVC";
            req.Body = "this is simple content";
            await _mailService.SendEmailAsync(req);
            return View();
                 
        }

        public IActionResult Register2()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register2([FromForm]UserView model)
        {
            if(ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email
                };
                IdentityResult Result=await _userManager.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    //add role
                    IdentityResult Result2 = await _userManager.AddToRoleAsync(user, "Member");
                    if(!Result2.Succeeded)
                    {
                        ModelState.AddModelError(nameof(LoginView.UserName), "failed to create an user");
                        return View(model);
                    }
                    //get token code
                    var usr = await _userManager.FindByEmailAsync(model.Email);
                    var userId = await _userManager.GetUserIdAsync(usr);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usr);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    string link = Url.Action("ConfirmEmail","Account",new { 
                    userid=userId,
                    code=code
                    },protocol:HttpContext.Request.Scheme);

                    //send email
                    MailRequest req = new MailRequest();
                    req.ToEmail = user.Email;
                    req.Subject = "confrimation email";
                    req.Body = "this is a confirmation email if you wanna confirm ur registration click on link below\n" + link;
                    await _mailService.SendEmailAsync(req);
                    return RedirectToAction("Index");



                }
                else
                {
                    ModelState.AddModelError(nameof(LoginView.UserName), "failed to create an user");
                }
            }
            return View();

        }
        public async Task<IActionResult> ConfirmEmail(string code, string userid)
        {
            //get token from code
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            //get user
            var user = await _userManager.FindByIdAsync(userid);
            //confirm email for registration
            IdentityResult Result = await _userManager.ConfirmEmailAsync(user, token);
            if (Result.Succeeded)
            {
                ViewBag.Status = "Email Confirmation is succeeded";
            }
            else
            {
                ViewBag.Status = "Email confirmation was failed or invalid token / userid";
            }
            return View();  
        }
        public IActionResult LoginRememberMe(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginView());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginRememberMe([FromForm] LoginView model,bool rememberMe,string returnUrl)
        {
            if(ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.UserName);
                if(user!=null)
                {
                    bool isValid = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(isValid)
                    {
                        var claims = new List<Claim>();
                        //store user name
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        //store user roles
                        var roles = await  _userManager.GetRolesAsync(user);
                        foreach(string role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        //create user Identity
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var authProperties = new AuthenticationProperties();
                        authProperties.IsPersistent = rememberMe;
                        authProperties.ExpiresUtc = DateTimeOffset.Now.AddDays(3);
                        //Cookie signed and store a cookie
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal, authProperties).Wait();
                        return Redirect(returnUrl ?? "/account");
                    }
                }
                ModelState.AddModelError(nameof(LoginView.UserName), 
                    "Invalid user/password or account is not activated");
            }
            return View(model);
        }


        public IActionResult RegisterCaptcha()
        {
            ViewData["RecaptchaKey"] = _recaptchaService.Configs.Key;
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> RegisterCaptcha([FromForm] UserView model )
        {
            ViewData["ReCaptchaKey"] = _recaptchaService.Configs.Key;
            if(ModelState.IsValid)
            {
                //validate recaptcha
                string token = Request.Form["g-recaptcha-response"];
                if(_recaptchaService.ValidateRecaptcha(token))
                {
                    ModelState.AddModelError(nameof(UserView.UserName), "You failed to captcha");
                    return View(model);
                }
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    

                };
                IdentityResult Result = await _userManager.CreateAsync(user,model.Password);
                if(Result.Succeeded)
                {
                    //add role 
                    IdentityResult Result2 = await _userManager.AddToRoleAsync(user, "Member");
                    if(Result2.Succeeded)
                        return RedirectToAction("/Account");
                }



            }
            return View(model);

        }
    }
}
