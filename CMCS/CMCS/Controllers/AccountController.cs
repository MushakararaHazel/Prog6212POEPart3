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

        public AccountController(AppDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Store minimal info in Session
            
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserId", user.Id.ToString());     // FIXED
            HttpContext.Session.SetString("UserRole", user.Role.ToString()); // FIXED



            // redirect based on role or back to ReturnUrl
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return user.Role switch
            {
                UserRole.Lecturer => RedirectToAction("Track", "Lecturer"),
                UserRole.Coordinator => RedirectToAction("Pending", "Coordinator"),
                UserRole.Manager => RedirectToAction("Approved", "Manager"),
                UserRole.HR => RedirectToAction("Index", "HR"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}

