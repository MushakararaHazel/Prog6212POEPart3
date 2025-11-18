using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CMCS.Controllers;
using CMCS.Data;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;



namespace CMCS.Tests
{
    public class LecturerControllerTests
    {
        [Fact]
        public void TotalAmount_ShouldBeCorrect()
        {

            var claim = new Claim
            {
                HoursWorked = 50,
                HourlyRate = 100
            };


            var total = claim.HoursWorked * claim.HourlyRate;


            Assert.Equal(5000, total);
        }


        [Fact]
        public void Notes_ShouldBeSavedCorrectly()
        {

            var claim = new Claim
            {
                Notes = "Lecturer added detailed notes about extra sessions."
            };

            Assert.Equal("Lecturer added detailed notes about extra sessions.", claim.Notes);
        }


        [Fact]
        public async Task Submit_ShouldAddClaim_AndRedirectToTrack()
        {
            // Arrange  
            var db = GetInMemoryDb();

            var mockService = new Mock<IClaimService>();
            mockService
                .Setup(s => s.SubmitClaimAsync(It.IsAny<Claim>(), It.IsAny<List<IFormFile>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Claim());

            var controller = new LecturerController(mockService.Object, db);
            controller.ModelState.Clear();

            var vm = new SubmitClaimVm
            {
                LecturerName = "Test Lecturer",
                LecturerId = "L2001",
                Month = new DateTime(2025, 10, 1),
                HoursWorked = 40,
                HourlyRate = 100,
                Notes = "Test Claim",
                FileUploads = new List<IFormFile>()
            };

            // Act
            var result = await controller.Submit(vm);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Track", redirect.ActionName);
        }
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

    }
}
