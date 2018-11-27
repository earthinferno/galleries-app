using GalleriesServer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace GalleriesUnitTests
{
    public class UnitTest1
    {
        private string _imageData = "[" +
            "{id:0,url:./../../src/img/index.png,comment:First Image,liked:True}," +
            "{id:1,url:./../../src/img/index.png,comment:Second Image,liked:True}," +
            "{id:2,url:./../../src/img/index.png,comment:Third Image,liked:True}," +
            "]";

        [Fact]
        public void Test1()
        {
            //setup
            var _contoller = new ImagesController();
            StringBuilder testString = new StringBuilder();
            testString.Append(_imageData);

            //Execute
            StringBuilder images = new StringBuilder();
            images.Append(_contoller.GetImages());

            //Test
            Assert.NotNull(images);
            Assert.Equal(testString.ToString(), images.ToString());
        }

        [Fact]
        public void TestAddImage()
        {
            //Setup
            var _contoller = new ImagesController();
            IFormFile imageFile = new FormFile(null, 0, 0, "File123", "File123.img");

            StringBuilder testString = new StringBuilder();
            testString.Append(_imageData.Substring(0, _imageData.Length-1));
            testString.Append("{id:3,url:./../../src/img/File123,comment:No Comment yet,liked:False},");
            testString.Append("]");

            //Execute
            StringBuilder images = new StringBuilder();
            images.Append(_contoller.UploadImage(imageFile));

            //Test
            Assert.NotNull(images);
            Assert.Equal(testString.ToString(), images.ToString());
        }
    }
}
