using GalleriesServer.Controllers;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GalleriesXUnitTest
{
    public class GalleriesControllerTests
    {
        //GalleriesDbContext _dbContextStub;
        //GalleriesController _controller;
        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;

        public GalleriesControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();

        }

        private (GalleriesDbContext, GalleriesController) Setup(string dbName)
        {
            _optionsBuilder.UseInMemoryDatabase(dbName);
            var dbContextStub = new GalleriesDbContext(_optionsBuilder.Options);
            return (
                dbContextStub,
                new GalleriesController(
                dbContextStub,
                new OwnerService(dbContextStub as GalleriesDbContext))
                );
        }

        [Fact]
        public async Task Tst_AddNewGallery_NoOwner_ReturnsNotOk()
        {
            // Arrange
            var (dbContextStub, controller) = Setup("testdbgalleries1");

            // Gallery
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser";
            DateTime createdDate = DateTime.Now;

            var gallery = new Gallery()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                UserId = externalUserId
            };

            // Act
            var response = await controller.PostGallery(gallery);

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);

        }

        [Fact]
        public async Task Tst_AddNewGallery_ReturnsOk()
        {
            // Arrange
            var (dbContextStub, controller) = Setup("testdbgalleries2");
            // Gallery
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser", 
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            var gallery = new Gallery() {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                UserId = externalUserId
            };

            // setup owner in database
            dbContextStub.Owners.Add(
                new Owner()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    ExternalIdentityProvider = externalIdentityProvider,
                    ExternalUserId = externalUserId
                });
            await dbContextStub.SaveChangesAsync();

            // Act
            var response = await controller.PostGallery(gallery);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response.Result);
            if (response.Result is CreatedAtActionResult result && result.Value is Gallery returnedGallery)
            {
                Assert.Equal(name, returnedGallery.Name);
                Assert.Equal(description, returnedGallery.Description);
                Assert.Equal(externalUserId, returnedGallery.UserId);
                Assert.Equal(createdDate, returnedGallery.CreatedDate);
            }
        }
    }
}
