using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthorizeRole("Lecturer")]
    public class LecturerController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IClaimService _service;

        public LecturerController(IClaimService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        
        public IActionResult Submit()
        {
            var fullName = HttpContext.Session.GetString("FullName");
            var lecturerId = HttpContext.Session.GetInt32("UserId");
            var rate = HttpContext.Session.GetString("HourlyRate");

            if (fullName == null || lecturerId == null)
                return RedirectToAction("Login", "Account");

            var vm = new SubmitClaimVm
            {
                LecturerName = fullName,
                LecturerId = lecturerId.Value.ToString(),
                HourlyRate = decimal.Parse(rate ?? "0")
            };

            ModelState.Clear();
            return View(vm);
        }


      
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
                    LecturerId = HttpContext.Session.GetString("Username"),  
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
            var username = HttpContext.Session.GetString("Username");

            if (username == null)
                return RedirectToAction("Login", "Account");

            var claims = await _service.GetMyClaimsAsync();

            claims = claims
                .Where(c => c.LecturerId == username)   
                .ToList();

            if (!string.IsNullOrWhiteSpace(searchLecturerId))
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


