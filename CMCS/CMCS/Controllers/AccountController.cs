using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
           
            var role = HttpContext.Session.GetString("UserRole");
            if (!string.IsNullOrEmpty(role))
            {
                return RedirectToRoleDashboard(role);
            }

            HttpContext.Session.Clear();
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                HttpContext.Session.Clear();
                return View();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                HttpContext.Session.Clear();
                return View();
            }

          
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);
             HttpContext.Session.SetString("UserRole", user.Role.ToString());  // FIXED
            HttpContext.Session.SetString("FullName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("HourlyRate", user.HourlyRate.ToString());

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoleDashboard(user.Role.ToString());
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
        private IActionResult RedirectToRoleDashboard(string role)
        {
            return role switch
            {
                nameof(UserRole.Lecturer)
                    => RedirectToAction("Submit", "Lecturer"),

                nameof(UserRole.Coordinator)
                    => RedirectToAction("Pending", "Coordinator"),

                nameof(UserRole.Manager)
                    => RedirectToAction("Approved", "Manager"),

                nameof(UserRole.HR)
                    => RedirectToAction("Index", "HR"),

                _ => RedirectToAction("Login", "Account")
            };
        }

    }
}
