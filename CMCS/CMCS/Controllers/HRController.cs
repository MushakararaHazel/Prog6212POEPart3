// Controllers/HRController.cs
using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthorizeRole("HR")]

    public class HRController : Controller
    {
        private readonly AppDbContext _db;
        public HRController(AppDbContext db) => _db = db;

        // List all users
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users.OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create() => View(new AppUser());

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if (!ModelState.IsValid) return View(user);

            // For the POE you can store password as plain text in PasswordHash
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
            if (!ModelState.IsValid) return View(user);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Simple report: total claimed per lecturer
        public async Task<IActionResult> Report()
        {
            var report = await _db.Claims
                .GroupBy(c => c.LecturerId)
                .Select(g => new
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

