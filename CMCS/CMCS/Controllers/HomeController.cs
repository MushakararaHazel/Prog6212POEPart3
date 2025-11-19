using System.Diagnostics;
using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalClaims = await _db.Claims.CountAsync();
            ViewBag.Pending = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Pending);
            ViewBag.Approved = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Approved);
            ViewBag.Rejected = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Rejected);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
