using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
       

        public AccountController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager=signInManager;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var isValidEmail = await _userManager.FindByEmailAsync(email) != null;

            if (!isValidEmail)
            {
                ModelState.AddModelError("", "Invalid email format or email not registered.");
                await Logger.LogAsync($"User '{email}' failed to log in.", _context);
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var isSignIn= await _signInManager.PasswordSignInAsync(user, password,false,false);
            if(isSignIn.Succeeded)
            {
                await Logger.LogAsync($"User '{email}' ;logged in.", _context);
                    return RedirectToAction("Index", "Home");
            }
            await Logger.LogAsync($"User '{email}' failed to log in.", _context);
            ModelState.AddModelError("", "Invalid email or password.");

           
            return View();
            //if (user != null)
            //{
            //    var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.Email),
            //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            //    };

            //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //    await HttpContext.SignInAsync(
            //        CookieAuthenticationDefaults.AuthenticationScheme,
            //        new ClaimsPrincipal(claimsIdentity));

            //    return RedirectToAction("Index", "Home");
            //}

            //ModelState.AddModelError("", "Invalid email or password.");
            //return View();
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await Logger.LogAsync($"User {_signInManager}logged out", _context);
            return RedirectToAction("Login");
        }
    }
}