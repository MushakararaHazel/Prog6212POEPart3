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

        // =============================================
        // GET Login – FIXES stale session showing old user
        // =============================================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If user is already logged in, redirect them properly
            var role = HttpContext.Session.GetString("UserRole");
            if (!string.IsNullOrEmpty(role))
            {
                return RedirectToRoleDashboard(role);
            }

            HttpContext.Session.Clear(); // Clear any stale session

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // =============================================
        // POST Login
        // =============================================
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
                HttpContext.Session.Clear(); // <--- IMPORTANT FIX
                return View();
            }

            // ========== Store Session ==========
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("FullName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("HourlyRate", user.HourlyRate.ToString());

            // If redirected from a protected page
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            // Redirect correctly by role
            return RedirectToRoleDashboard(user.Role.ToString());
        }

        // =============================================
        // LOGOUT
        // =============================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // =============================================
        // ROUTE REDIRECTOR (Fixes your issue)
        // =============================================
        private IActionResult RedirectToRoleDashboard(string role)
        {
            return role switch
            {
                nameof(UserRole.Lecturer) => RedirectToAction("Submit", "Lecturer"),
                nameof(UserRole.Coordinator) => RedirectToAction("Pending", "Coordinator"),
                nameof(UserRole.Manager) => RedirectToAction("Approved", "Manager"),
                nameof(UserRole.HR) => RedirectToAction("Index", "HR"),
                _ => RedirectToAction("Login")
            };
        }
    }
}
