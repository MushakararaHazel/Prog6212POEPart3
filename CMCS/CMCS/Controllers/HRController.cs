using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using CMCS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthorizeRole("HR")]
    public class HRController : Controller
    {
        private readonly AppDbContext _db;
        public HRController(AppDbContext db) => _db = db;

        // Show all users
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Username)
                .ToListAsync();

            return View(users);
        }

        // -----------------------
        // CREATE USER
        // -----------------------

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AppUser());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            // Required POE password (no hashing)
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Password is required.");
                return View(user);
            }

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["Success"] = "User created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // -----------------------
        // EDIT USER
        // -----------------------

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existing = await _db.Users.FindAsync(user.Id);
            if (existing == null) return NotFound();

            // Update fields but keep old password if left blank
            existing.Username = user.Username;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.HourlyRate = user.HourlyRate;
            existing.Role = user.Role;

            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                existing.PasswordHash = user.PasswordHash;   // replace
            }

            await _db.SaveChangesAsync();

            TempData["Success"] = "User updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // -----------------------
        // DELETE USER
        // -----------------------

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = "User deleted.";
            return RedirectToAction(nameof(Index));
        }

        // -----------------------
        // REPORTS
        // -----------------------

        public async Task<IActionResult> Report()
        {
            var report = await _db.Claims
                .GroupBy(c => c.LecturerId)
                .Select(g => new ClaimReportVm
                {
                    LecturerId = g.Key,
                    TotalAmount = g.Sum(x => x.HoursWorked * x.HourlyRate),
                    ClaimCount = g.Count()
                })
                .ToListAsync();

            return View(report);
        }
    }
}
