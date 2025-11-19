using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Reports;
using CMCS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace CMCS.Controllers
{
    [AuthorizeRole("HR")]
    public class HRController : Controller
    {
        private readonly AppDbContext _db;
        public HRController(AppDbContext db) => _db = db;

      
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Username)
                .ToListAsync();

            return View(users);
        }

     
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new AppUser());
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(AppUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Password is required.");
                return View(model);
            }

            _db.Users.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "User created successfully!";
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existing = await _db.Users.FindAsync(user.Id);
            if (existing == null) return NotFound();

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.HourlyRate = user.HourlyRate;
            existing.Role = user.Role;

            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                existing.PasswordHash = user.PasswordHash;

            await _db.SaveChangesAsync();

            TempData["Success"] = "User updated successfully!";
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

            TempData["Success"] = "User deleted.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GeneratePdf()
        {
            var users = await _db.Users.ToListAsync();

            var vm = new ReportVm
            {
                Users = users
            };

            var pdfBytes = new ReportDocument(vm).GeneratePdf();

            return File(pdfBytes, "application/pdf", "UserReport.pdf");
        }

    }
}

