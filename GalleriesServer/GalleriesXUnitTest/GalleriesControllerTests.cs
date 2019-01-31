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
    public class GalleriesControllerTests
    {
        //GalleriesDbContext _dbContextStub;
        //GalleriesController _controller;
        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;
        HttpContext _httpContext;

        public GalleriesControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");



        }
/*
        private (GalleriesDbContext, GalleriesController) Setup(string dbName)
        {
            _optionsBuilder.UseInMemoryDatabase(dbName);
            var dbContextStub = new GalleriesDbContext(_optionsBuilder.Options);
            return (
                dbContextStub,
                new GalleriesController(
                dbContextStub,
                new OwnerService(dbContextStub as GalleriesDbContext), 
                new MediaContainerService(dbContextStub))
                );
        }
*/
        private GalleriesController Setup(GalleriesDbContext dbContext)
        {
            return (
                new GalleriesController(
                dbContext,
                new OwnerService(dbContext as GalleriesDbContext),
                new MediaContainerService(dbContext as GalleriesDbContext)
                ));
        }


        [Fact]
        public async Task Tst_AddNewGalleryNoOwner_Returns400()
        {
            // ARRANGE
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

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // ACT
            ActionResult<MediaContainer> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                response = await controller.PostGallery(gallery);
            }

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);

        }

        [Fact]
        public async Task Tst_AddNewGalleryOk_Returns201()
        {
            // ARRANGE
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            /*
            var gallery = new Gallery()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                UserId = externalUserId
            };
            */
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
            ActionResult<MediaContainer> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PostGallery(new Gallery()
                {
                    Name = name,
                    CreatedDate = createdDate,
                    Description = description,
                    UserId = externalUserId
                });
            }

            // ASSERT
            Assert.IsType<CreatedAtActionResult>(response.Result);
            if (response.Result is CreatedAtActionResult result && result.Value is Gallery returnedGallery)
            {
                Assert.Equal(name, returnedGallery.Name);
                Assert.Equal(description, returnedGallery.Description);
                Assert.Equal(externalUserId, returnedGallery.UserId);
                Assert.Equal(createdDate, returnedGallery.CreatedDate);
            }
        }


        [Fact]
        public async Task Tst_GetGalleriesOg_Returns200()
        {
            // ARRANGE
            // Gallery
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com",
                filename = "testfilename",
                filecomment = "testfilecomment";
            DateTime createdDate = DateTime.Now;

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var owner = new Owner()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    ExternalIdentityProvider = externalIdentityProvider,
                    ExternalUserId = externalUserId
                };

                var mediaItems = new List<MediaItem>
            {
                new MediaItem {FileName = filename, Comment = filecomment}
            };

                var mediaContainer = new MediaContainer()
                {
                    Name = name,
                    CreatedDate = createdDate,
                    Description = description,
                    Owner = owner,
                    MediaItems = mediaItems
                };

                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                dbContext.MediaContainers.Add(mediaContainer);
                await dbContext.SaveChangesAsync();
            }


            // ACT
            ActionResult<IEnumerable<MediaContainer>> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.GetGalleries(externalUserId);
            }
                

            // ASSERT
            if (response.Value is List<MediaContainer> gallery && gallery[0].MediaItems is List<MediaItem> items)
            {
                Assert.Equal(name, gallery[0].Name);
                Assert.Equal(description, gallery[0].Description);
                Assert.Equal(externalUserId, gallery[0].Owner.ExternalUserId);
                Assert.Equal(createdDate, gallery[0].CreatedDate);
                Assert.Equal(filename, items[0].FileName);
                Assert.Equal(filecomment, items[0].Comment);
            }
        }

        [Fact]
        public async Task Tst_GetEmptyGalleriesOk_Returns200()
        {

            // ARRANGE
            string externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var owner = new Owner()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    ExternalIdentityProvider = externalIdentityProvider,
                    ExternalUserId = externalUserId
                };

                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                await dbContext.SaveChangesAsync();

            }

            // ACT
            ActionResult<IEnumerable<MediaContainer>> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.GetGalleries(externalUserId);
            }
               

            // ASSERT
            if (response.Value is List<MediaContainer> returnedGallery)
            {
                Assert.Empty(returnedGallery);
            }
        }


        [Fact]
        public async Task Tst_UpdateGalleriesNotFound_Return409 ()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };

            var mediaContainer = new MediaContainer()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                Owner = owner
            };

            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {

                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                dbContext.MediaContainers.Add(mediaContainer);
                dbContext.SaveChanges();
            }

            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                controller.ControllerContext.HttpContext = _httpContext;

                mediaContainer.ID = -99;
                response = await controller.PutGallery(mediaContainer.ID, mediaContainer);
            }
                
            //Assert
            Assert.IsType<ConflictObjectResult>(response);
        }

        [Fact]
        public async Task Tst_UpdateGalleryDataOk_Return204()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            string updatedName = name + "@@",
                updatedDescription = description + "@@";

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };

            var mediaContainer = new MediaContainer()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                Owner = owner
            };

            MediaContainer mediaContainerUpdate;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                dbContext.MediaContainers.Add(mediaContainer);
                dbContext.SaveChanges();

                mediaContainerUpdate = await dbContext.MediaContainers.FirstAsync();
                mediaContainerUpdate.Name = updatedName;
                mediaContainerUpdate.Description = updatedDescription;
//            }

            // ACT
            ActionResult response;
//            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
//            {
                var controller = Setup(dbContext);
                controller.ControllerContext.HttpContext = _httpContext;
                response = await controller.PutGallery(mediaContainerUpdate.ID, mediaContainerUpdate);

//            }

            // ASSERT
            Assert.IsType<NoContentResult>(response);
//            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
//            {
                var updatedGallery = dbContext.MediaContainers.Find(mediaContainerUpdate.ID);
                Assert.IsType<NoContentResult>(response);
                Assert.Equal(updatedName, updatedGallery.Name);
                Assert.Equal(updatedDescription, updatedGallery.Description);
                Assert.NotNull(updatedGallery.Owner);
                Assert.Equal(owner.ID, updatedGallery.Owner.ID);
            }
        }

        [Fact]
        public async Task Tst_DeleteGalleriesDeletedOk_Returns204()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // Gallery
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };

            var mediaContainer = new MediaContainer()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                Owner = owner
            };

            int id;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                dbContext.MediaContainers.Add(mediaContainer);
                await dbContext.SaveChangesAsync();

               id = dbContext.MediaContainers.FirstAsync().Result.ID;
            }


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.DeleteGallery(id);
            }

            // ASSERT
            Assert.IsType<NoContentResult>(response);
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                if (await dbContext.MediaContainers.FindAsync(id) is MediaContainer returnedGallery)
                {
                    Assert.Equal(name, returnedGallery.Name);
                    Assert.Equal(description, returnedGallery.Description);
                    Assert.Equal(externalUserId, returnedGallery.Owner.ExternalUserId);
                    Assert.Equal(createdDate, returnedGallery.CreatedDate);
                }
            }
        }

        [Fact]
        public async Task Tst_DeleteGalleriesNotFound_returns404()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // Gallery
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com";
            DateTime createdDate = DateTime.Now;

            var owner = new Owner()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                ExternalIdentityProvider = externalIdentityProvider,
                ExternalUserId = externalUserId
            };

            var mediaContainer = new MediaContainer()
            {
                Name = name,
                CreatedDate = createdDate,
                Description = description,
                Owner = owner
            };

            int id;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                // setup owner in database
                dbContext.Owners.Add(owner);
                // setup gallery in the database
                dbContext.MediaContainers.Add(mediaContainer);
                await dbContext.SaveChangesAsync();

                id = dbContext.MediaContainers.FirstAsync().Result.ID;
            }

            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                
                var controller = Setup(dbContext);
                response = await controller.DeleteGallery(id + 99);
            }
            // ASSERT
            Assert.IsType<NotFoundResult>(response);
        }


    }
}
