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

namespace GalleriesUnitTests
{
    public class NewMediaControllerTests
    {
        HttpContext _httpContextMock = new Mock<HttpContext>().Object;
        DbContextOptionsBuilder _dbContextOptionsBuilderMock = new Mock<IDbContextOptionsBuilderInfrastructure>().Object as DbContextOptionsBuilder;
        IImageStore _imageStoreMock;
        GalleriesDbContext _dbContextStub;
        MediaController _controller;


        public NewMediaControllerTests()
        {
            var imageStoreMock = new Mock<IImageStore>();
            imageStoreMock.Setup(a => a.SaveImage(It.IsAny<Stream>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid().ToString());
            _imageStoreMock = imageStoreMock.Object;

            // Construct the media controller object
            var optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            optionsBuilder.UseInMemoryDatabase("testdb");

            _dbContextStub = new GalleriesDbContext(optionsBuilder.Options);

            _controller = new MediaController(
                _dbContextStub,
                _imageStoreMock,
                new OwnerService(_dbContextStub as GalleriesDbContext),
                new MediaContainerService(_dbContextStub as GalleriesDbContext),
                new MediaItemService(_dbContextStub as GalleriesDbContext));
    
        }
        
        /*
        [Fact]
        public void templatetest()
        {

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");


            var collection = StubFileUpload(folderName: "testFolder", comment: "testComment", userId: "testUser1"); ;

            // Act
            _controller.ControllerContext.HttpContext = _httpContextMock;
            //_controller.ControllerContext = httpContext;
            var okResponse = _controller.Upload(collection);

            //Assert
            Assert.IsType<CreatedAtActionResult>(okResponse);

        }
        */

        [Fact]
        public async Task tst_FileUploadFail_IfNoUserAsync()
        {

            /*
            var optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            optionsBuilder.UseInMemoryDatabase("testdb");

            var _dbContextStub = new GalleriesDbContext(optionsBuilder.Options);
            var _controller = new MediaController(_dbContextStub, _imageStoreMock, new OwnerService(_dbContextStub as GalleriesDbContext), new MediaContainerService(_dbContextStub as GalleriesDbContext));
            */
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");


            var collection = StubFileUpload(folderName: "testFolder", comment: "testComment", userId: "testUser1"); ;

            // Act
            _controller.ControllerContext.HttpContext = _httpContextMock;
            var badRequest = await _controller.Upload(collection);

            //Assert
            Assert.IsType<BadRequestResult>(badRequest.Result);

        }

        [Fact]
        public async Task tst_FileUploadFail_IfNoFolderAsync()
        {
            //arrange
            /*
            var optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            optionsBuilder.UseInMemoryDatabase("testdb");

            var _dbContextStub = new GalleriesDbContext(optionsBuilder.Options);
            var _controller = new MediaController(_dbContextStub, _imageStoreMock, new OwnerService(_dbContextStub as GalleriesDbContext), new MediaContainerService(_dbContextStub as GalleriesDbContext));
            */
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");

            var collection = StubFileUpload(folderName: "testFolder", comment: "testComment", userId: "testUser1");

            _dbContextStub.Owners.Add(new Owner() { ExternalUserId = "testUser1", FirstName = "test", LastName = "test", EmailAddress = "undefined", ExternalIdentityProvider = "Undefined" });
            await _dbContextStub.SaveChangesAsync();

            _controller.ControllerContext.HttpContext = _httpContextMock;

            // Act
            var badRequest = await _controller.Upload(collection);

            //Assert
            Assert.IsType<BadRequestResult>(badRequest.Result);

        }

        [Fact]
        public async Task tst_FileUploadSucced()
        {
            //arrange
            /*
             * var optionsBuilder = new DbContextOptionsBuilder<GalleriesDbContext>();
            optionsBuilder.UseInMemoryDatabase("testdb");

            var _dbContextStub = new GalleriesDbContext(optionsBuilder.Options);
            var _controller = new MediaController(
                _dbContextStub, 
                _imageStoreMock, 
                new OwnerService(_dbContextStub as GalleriesDbContext), 
                new MediaContainerService(_dbContextStub as GalleriesDbContext),
                new MediaItemService(_dbContextStub as GalleriesDbContext));
            */

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");

            var folderName = "testFolder";
            var username = "testUser1";
            var collection = StubFileUpload(folderName: folderName, comment: "testComment", userId: username);

            _dbContextStub.MediaContainers.Add(new MediaContainer() {
                CreatedDate = DateTime.Now, Description ="TestDescription", Name= folderName,
                Owner = new Owner()
                {
                    ExternalUserId = username,
                    FirstName = "test",
                    LastName = "test",
                    EmailAddress = "undefined",
                    ExternalIdentityProvider = "Undefined"
                }
            });
            await _dbContextStub.SaveChangesAsync();

            _controller.ControllerContext.HttpContext = _httpContextMock;

            // Act
            var response = await _controller.Upload(collection);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response.Result);
            if (response.Result is CreatedAtActionResult result && result.Value is List<MediaItem> items)
            {
                Assert.Equal("testComment", items[0].Comment);
                Assert.Equal("index.png", items[0].FileName);
                Assert.NotNull(items[0].ImageUri);
                Assert.NotEqual(0,items[0].ID);
            }
        }


        private FormCollection StubFileUpload(string folderName, string comment, string userId)
        {
            var dic = new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            dic.Add("UserFolder", folderName);
            dic.Add("Comment", comment);
            dic.Add("UserId", userId);

            //var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "index.png");

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
    }
}
