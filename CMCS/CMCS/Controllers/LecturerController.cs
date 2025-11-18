using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthorizeRole("IC")]
    public class LecturerController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IClaimService _service;

        public LecturerController(IClaimService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        // -----------------------------------------
        // STEP 6 — Auto-fill lecturer details
        // -----------------------------------------
        public IActionResult Submit()
        {
            var lecturerName = HttpContext.Session.GetString("Username");
            var lecturerId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(lecturerName) || string.IsNullOrEmpty(lecturerId))
            {
                return RedirectToAction("Login", "Account");
            }

            var vm = new SubmitClaimVm
            {
                LecturerName = lecturerName,
                LecturerId = lecturerId
            };

            return View(vm);
        }

        // POST Submit
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmitClaimVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var claim = new Claim
                {
                    LecturerName = vm.LecturerName,
                    LecturerId = vm.LecturerId,
                    Month = vm.Month,
                    HoursWorked = vm.HoursWorked,
                    HourlyRate = vm.HourlyRate,
                    Notes = vm.Notes,
                    Status = ClaimStatus.Pending
                };

                await _service.SubmitClaimAsync(claim, vm.FileUploads);

                TempData["Success"] = "Claim submitted successfully!";
                return RedirectToAction("Track");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Track(string? searchLecturerId = null)
        {
            var claims = await _service.GetMyClaimsAsync();

            if (!string.IsNullOrEmpty(searchLecturerId))
            {
                claims = claims
                    .Where(c => c.LecturerId.Contains(searchLecturerId, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = await _service.GetClaimByIdAsync(id);
            if (claim == null) return NotFound();

            await _service.DeleteClaimAsync(id);
            TempData["Success"] = "Claim deleted successfully.";
            return RedirectToAction("Track");
        }
    }
}


