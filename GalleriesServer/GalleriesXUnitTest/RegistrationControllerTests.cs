using GalleriesServer.Controllers;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GalleriesXUnitTest
{
    public class RegistrationControllerTests
    {
        //GalleriesDbContext _dbContextStub;
        //RegistrationController _controller;
        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;

        public RegistrationControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            /*
            // Construct the media controller object
            var optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            optionsBuilder.UseInMemoryDatabase("testdbregistration");

            _dbContextStub = new GalleriesDbContext(optionsBuilder.Options);

            _controller = new RegistrationController(
                _dbContextStub,
                new OwnerService(_dbContextStub as GalleriesDbContext));
                */
        }

        private (GalleriesDbContext, RegistrationController) Setup(string dbName)
        {
            _optionsBuilder.UseInMemoryDatabase(dbName);
            var dbContextStub = new GalleriesDbContext(_optionsBuilder.Options);
            return (
                dbContextStub,
                new RegistrationController(
                dbContextStub,
                new OwnerService(dbContextStub as GalleriesDbContext))
                );
        }

        [Fact]
        public async Task Tst_RegisterDetails_Ok()
        {
            // ACT
            var (dbContextStub, controller) = Setup("testdbregister1");

            var response = await controller.PostOwner(
                new Owner() {
                    FirstName = "testFirstName",
                    LastName = "testLastName",
                    EmailAddress = "testuser@test.com",
                    ExternalIdentityProvider = "testprovider",
                    ExternalUserId = "testUser" }
                );

            // ASSERT
            Assert.IsType<CreatedAtActionResult>(response.Result);
            if (response.Result is CreatedAtActionResult result && result.Value is Owner owner)
            {
                Assert.Equal("testFirstName", owner.FirstName);
                Assert.Equal("testLastName", owner.LastName);
                Assert.Equal("testuser@test.com", owner.EmailAddress);
                Assert.Equal("testprovider", owner.ExternalIdentityProvider);
                Assert.Equal("testUser", owner.ExternalUserId);
            }
        }

        [Fact]
        public async Task Tst_RegisterDuplicateDetails_NotOk()
        {

            // ARRANGE
            var (dbContextStub, controller) = Setup("testdbregister2");

            string firstName = "testFirstName",
                   lastName = "testLastName",
                   emailAddress = "testuser@test.com",
                   externalIdentityProvider = "testprovider",
                   externalUserId = "testUser";

            dbContextStub.Owners.Add(
                new Owner() {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    ExternalIdentityProvider = externalIdentityProvider,
                    ExternalUserId = externalUserId });
            await dbContextStub.SaveChangesAsync();

            // ACT
            var response = await controller.PostOwner(
                new Owner()
                {
                    FirstName = firstName + "@@",
                    LastName = lastName + "@@",
                    EmailAddress = emailAddress, //unique bit
                    ExternalIdentityProvider = externalIdentityProvider + "@@",
                    ExternalUserId = externalUserId + "@@"
                }
                );

            // ASSERT
            Assert.IsType<BadRequestResult>(response.Result);
        }
    }
}
