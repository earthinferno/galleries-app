using GalleriesServer.Controllers;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GalleriesXUnitTest
{
    public class AccountsControllerTests
    {

        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;
        HttpContext _httpContext;

        public AccountsControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");

        }

        private AccountsController Setup(GalleriesDbContext dbContext)
        {
            return (
                new AccountsController(
                dbContext,
                new OwnerService(dbContext as GalleriesDbContext)
                ));
        }

        [Fact]
        public async Task tst_GetAccount_Returns404()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // ACT
            ActionResult<Owner> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                response = await controller.GetAccount("NoTestAccount");
            }

            // ACT
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task tst_GetAccount_Returns200()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";

            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                dbContext.Owners.Add(new Owner
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

                response = await controller.GetAccount(externalUserId);
            }

            // ACT
            Assert.IsType<Owner>(response.Value);

            Assert.Equal(firstName, response.Value.FirstName);
            Assert.Equal(lastName, response.Value.LastName);
            Assert.Equal(externalIdentityProvider, response.Value.ExternalIdentityProvider);
            Assert.Equal(emailAddress, response.Value.EmailAddress);            
        }









        [Fact]
        public async Task tst_UpdateOwnerdata_Return409()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PutAccount(externalUserId + "@@", owner);
            }

            // ASSERT
            Assert.IsType<BadRequestResult>(response);

        }

        [Fact]
        public async Task tst_UpdateOwner_Return400()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PutAccount(externalUserId, owner);
            }

            // ASSERT
            Assert.IsType<ConflictObjectResult>(response);
        }

        [Fact]
        public async Task tst_UpdateOwnerdataOk_Return204()
        {

            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };


            //setup data
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                // setup owner in database
                dbContext.Owners.Add(owner);
                await dbContext.SaveChangesAsync();

                owner = await dbContext.Owners.FirstAsync();
            }

            string firstNameUpdated = firstName + "@@",
                lastNameUpdated = lastName + "@@",
                externalIdentityProviderUpdated = externalIdentityProvider + "@@",
                emailAddressUpdated = emailAddress + "@@";

            owner.FirstName = firstNameUpdated;
            owner.LastName = lastNameUpdated;
            owner.ExternalIdentityProvider = externalIdentityProviderUpdated;
            owner.EmailAddress = emailAddressUpdated;


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PutAccount(externalUserId, owner);
            }


            // ASSERT
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                Assert.IsType<NoContentResult>(response);
                if (await dbContext.Owners.FirstAsync() is Owner dbOwner)
                {
                    Assert.Equal(owner.FirstName, dbOwner.FirstName);
                    Assert.Equal(owner.LastName, dbOwner.LastName);
                    Assert.Equal(owner.ExternalIdentityProvider, dbOwner.ExternalIdentityProvider);
                    Assert.Equal(owner.ExternalUserId, dbOwner.ExternalUserId);
                    Assert.Equal(owner.EmailAddress, dbOwner.EmailAddress);
                }
            }
        }

        [Fact]
        public async Task tst_DeleteAccount_Return400()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.DeleteAccount(99);
            }

            // ASSERT
            Assert.IsType<ConflictObjectResult>(response);
        }

        [Fact]
        public async Task tst_DeleteOwnerdataOk_Return204()
        {

            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };


            //setup data
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                // setup owner in database
                dbContext.Owners.Add(owner);
                await dbContext.SaveChangesAsync();

                owner = await dbContext.Owners.FirstAsync();
            }


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.DeleteAccount(owner.ID);
            }


            // ASSERT
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                Assert.IsType<NoContentResult>(response);
                var ownerCount = await dbContext.Owners.CountAsync();
                Assert.Equal(0, ownerCount);
            }
        }
    }
}
