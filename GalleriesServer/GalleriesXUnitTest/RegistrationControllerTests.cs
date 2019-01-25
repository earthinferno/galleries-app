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
        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;

        public RegistrationControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
        }

        private RegistrationController Setup(GalleriesDbContext dbContext)
        {
            return (
                new RegistrationController(
                    dbContext,
                    new OwnerService(dbContext as GalleriesDbContext))
                );
        }

        [Fact]
        public async Task Tst_RegisterDetails_Ok()
        {
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                // ACT
                var response = await controller.PostOwner(
                    new Owner()
                    {
                        FirstName = "testFirstName",
                        LastName = "testLastName",
                        EmailAddress = "testuser@test.com",
                        ExternalIdentityProvider = "testprovider",
                        ExternalUserId = "testUser"
                    });

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

        }

        [Fact]
        public async Task Tst_RegisterDuplicateEmailAddress_NotOkay()
        {
            // Arrange
            string firstName = "testFirstName",
               lastName = "testLastName",
               emailAddress = "testuser@test.com",
               externalIdentityProvider = "testprovider",
               externalUserId = "testUser";

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                dbContext.Owners.Add(
                    new Owner()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        EmailAddress = emailAddress,
                        ExternalIdentityProvider = externalIdentityProvider,
                        ExternalUserId = externalUserId
                    });
                await dbContext.SaveChangesAsync();
            }


            // ACT
            ActionResult<Owner> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                response = await controller.PostOwner(
                    new Owner()
                    {
                        FirstName = firstName + "@@",
                        LastName = lastName + "@@",
                        EmailAddress = emailAddress, //duplicate item
                        ExternalIdentityProvider = externalIdentityProvider + "@@",
                        ExternalUserId = externalUserId + "@@"
                    });
            }

            // ASSERT
            Assert.IsType<BadRequestObjectResult>(response.Result);
            if (response.Result is BadRequestObjectResult result && result.Value is string message)
            {
                Assert.Equal("Email address already in use. Value: " + emailAddress, message);
            }
            
        }

        [Fact]
        public async Task Tst_RegisterDuplicateExternalUserid_NotOk()
        {
            // ARRANGE
            string firstName = "testFirstName",
               lastName = "testLastName",
               emailAddress = "testuser@test.com",
               externalIdentityProvider = "testprovider",
               externalUserId = "testUser";

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                dbContext.Owners.Add(
                    new Owner()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        EmailAddress = emailAddress,
                        ExternalIdentityProvider = externalIdentityProvider,
                        ExternalUserId = externalUserId
                    });
                await dbContext.SaveChangesAsync();
            }

            // ACT
            ActionResult<Owner> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                response = await controller.PostOwner(
                    new Owner()
                    {
                        FirstName = firstName + "@@",
                        LastName = lastName + "@@",
                        EmailAddress = emailAddress + "@@",
                        ExternalIdentityProvider = externalIdentityProvider + "@@",
                        ExternalUserId = externalUserId //duplicate item
                    });
            }


            // ASSERT
            Assert.IsType<BadRequestObjectResult>(response.Result);
            if (response.Result is BadRequestObjectResult result && result.Value is string message)
            {
                Assert.Equal("User account already exists. Value: " + externalUserId, message);
            }

        }

    }
}
