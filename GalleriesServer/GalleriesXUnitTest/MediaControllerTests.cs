using GalleriesServer.Controllers;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GalleriesXUnitTest
{
    public class MediaControllerTests
    {

        DbContextOptionsBuilder<GalleriesDbContext> _optionsBuilder;
        HttpContext _httpContext;

        public MediaControllerTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");

        }

        private MediaController Setup(GalleriesDbContext dbContext)
        {
            var imageStoreMock = new Mock<IImageStore>();
            imageStoreMock.Setup(a => a.SaveImage(It.IsAny<Stream>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid().ToString());
            return (
                new MediaController(
                dbContext,
                imageStoreMock.Object,
                new OwnerService(dbContext as GalleriesDbContext),
                new MediaContainerService(dbContext as GalleriesDbContext),
                new MediaItemService(dbContext as GalleriesDbContext)
                ));
        }


        [Fact]
        public async Task tst_FileUploadNoAccount_Returns400()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var collection = StubFileUpload(folderName: "testFolder", comment: "testComment", userId: "testUser1"); ;

            // ACT
            ActionResult<MediaItem> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                // Act
                controller.ControllerContext.HttpContext = _httpContext;
                response = await controller.Upload(collection);
            }

            // ASSERT
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task tst_FileUploadNoGallery_Returns400()
        {
            // ARRANGE
            var collection = StubFileUpload(folderName: "testFolder", comment: "testComment", userId: "testUser1");

            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                dbContext.Owners.Add(new Owner() { ExternalUserId = "testUser1", FirstName = "test", LastName = "test", EmailAddress = "undefined", ExternalIdentityProvider = "Undefined" });
                await dbContext.SaveChangesAsync();
            }

            // ACT
            ActionResult<MediaItem> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                controller.ControllerContext.HttpContext = _httpContext;

                response = await controller.Upload(collection);
            }

            // ASSERT
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task tst_FileUploadSucceed_Returns200()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            //Variables
            var folderName = "testFolder";
            var username = "testUser1";
            var collection = StubFileUpload(folderName: folderName, comment: "testComment", userId: username);

            //Arrange data
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                dbContext.MediaContainers.Add(new MediaContainer()
                {
                    CreatedDate = DateTime.Now,
                    Description = "TestDescription",
                    Name = folderName,
                    Owner = new Owner()
                    {
                        ExternalUserId = username,
                        FirstName = "test",
                        LastName = "test",
                        EmailAddress = "undefined",
                        ExternalIdentityProvider = "Undefined"
                    }
                });
                await dbContext.SaveChangesAsync();
            }

            // ACT
            ActionResult<MediaItem> response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                //Arrange
                var controller = Setup(dbContext);
                controller.ControllerContext.HttpContext = _httpContext;

                // Act
                response = await controller.Upload(collection);
            }

            // ASSERT
            Assert.IsType<CreatedAtActionResult>(response.Result);
            if (response.Result is CreatedAtActionResult result && result.Value is List<MediaItem> items)
            {
                Assert.Equal("testComment", items[0].Comment);
                Assert.Equal("index.png", items[0].FileName);
                Assert.NotNull(items[0].ImageUri);
                Assert.NotEqual(0, items[0].ID);
            }
        }


        [Fact]
        public async Task tst_UpdateItemMetadata_Return409()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            string filename = "testfilename",
                filecomment = "testfilecomment";
            var item = new MediaItem { ID = 1, FileName = filename, Comment = filecomment };


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PutMedia(item.ID, item);
            }

            // ASSERT
            Assert.IsType<ConflictObjectResult>(response);
            
        }

        [Fact]
        public async Task tst_UpdateItemMetadata_Return400()
        {
            // ARRANGE
            string filename = "testfilename",
                filecomment = "testfilecomment";
            var item = new MediaItem { ID = 1, FileName = filename, Comment = filecomment };
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.PutMedia(item.ID + -99, item);
            }

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task tst_UpdateItemMetadataOk_Return204()
        {

            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // Variables
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com",
                filename = "testfilename",
                filecomment = "testfilecomment";
            var createdDate = DateTime.Now;

            //setup data
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
            ActionResult response;
            MediaItem item;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                item = await dbContext.MediaItems.FirstAsync();
                item.FileName = item.FileName + "@@";
                item.Comment = item.Comment + "@@";


                response = await controller.PutMedia(item.ID, item);
            }

            // ASSERT
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            { 
                Assert.IsType<NoContentResult>(response);
                if (await dbContext.MediaItems.FirstAsync() is MediaItem mediaItem)
                {
                    Assert.Equal(item.FileName, mediaItem.FileName);
                    Assert.Equal(item.Comment, mediaItem.Comment);
                }
            }
        }

        [Fact]
        public async Task tst_DeleteItemMetadata_Return409()
        {
            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            string filename = "testfilename",
                filecomment = "testfilecomment";
            var item = new MediaItem { ID = 1, FileName = filename, Comment = filecomment };


            // ACT
            ActionResult response;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);
                response = await controller.DeleteMedia("nouser", item.ID);
            }

            // ASSERT
            Assert.IsType<ConflictObjectResult>(response);

        }


        [Fact]
        public async Task tst_DeleteItemMetadataOk_Return204()
        {

            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // Variables
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com",
                filename = "testfilename",
                filecomment = "testfilecomment";
            var createdDate = DateTime.Now;

            //setup data
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
            ActionResult response;
            MediaItem item;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                item = await dbContext.MediaItems.FirstAsync();

                response = await controller.DeleteMedia(externalUserId, item.ID);
            }

            // ASSERT
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                Assert.IsType<NoContentResult>(response);
                Assert.Null(await dbContext.MediaItems.FindAsync(item.ID));
            }
        }

        private FormCollection StubFileUpload(string folderName, string comment, string userId)
        {
            var dic = new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            dic.Add("UserFolder", folderName);
            dic.Add("Comment", comment);
            dic.Add("UserId", userId);

            FormFile file;
            using (var stream = File.OpenRead("./Data/index.png"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/png"
                }; ;
            }

            return new FormCollection(dic, new FormFileCollection { file });
        }

        [Fact]
        public async Task tst_GetAllGalleryItems_Return200()
        {

            // ARRANGE
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            // Variables
            string name = "testGallery1",
                description = "Test description 1",
                externalUserId = "testUser",
                firstName = "testFirstName",
                lastName = "testLastName",
                externalIdentityProvider = "testprovider",
                emailAddress = "testuser@test.com",
                filename = "testfilename",
                filecomment = "testfilecomment";
            var createdDate = DateTime.Now;

            //setup data
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
            ActionResult<List <BlobItem>> response;
            MediaContainer item;
            using (var dbContext = new GalleriesDbContext(_optionsBuilder.Options))
            {
                var controller = Setup(dbContext);

                item = await dbContext.MediaContainers.FirstAsync();

                response = await controller.GetItems(item.ID, externalUserId);
            }

            // ASSERT
            Assert.IsType<OkObjectResult>(response);
            if (response.Result is OkObjectResult result && result.Value is List<BlobItem> items)
            {
                Assert.NotNull(items[0].BlobName);
            }
            
        }

    }
}
