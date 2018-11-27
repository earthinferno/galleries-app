using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleriesServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalleriesServer.Controllers
{
    public class ImagesController : Controller
    {
        private int _imageId = 0;
        private IList<Image> _data; 
        public ImagesController()
        {
            _data = new List<Image>()
            {
                new Image{Id = _imageId++, Comment="First Image", Liked = true, Url="./../../src/img/index.png" },
                new Image{Id = _imageId++, Comment="Second Image", Liked = true, Url="./../../src/img/index.png" },
                new Image{Id = _imageId++, Comment="Third Image", Liked = true, Url="./../../src/img/index.png" }
            };
        }

        [HttpGet]
        public string GetImages()
        {
            return GetImages(_data);
        }

        [HttpPost]
        public string UploadImage (IFormFile image)
        {
            var images = _data.Select(x => x).ToList();
            images.Add(new Image() { Id = _imageId++, Comment = "No Comment yet", Liked = false, Url = "./../../src/img/" + image.Name });
            return GetImages(images);
        }

        private string GetImages(IList<Image> images)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            str.Append(images.Select(x => ImageTemplate(x))
                .Aggregate((curr, next) => curr + "," + next) + ",]"
                );
            return str.ToString();
        }

        private string ImageTemplate(Image image)
        {
            return "{id:" + image.Id.ToString() + "," +
                   "url:" + image.Url + "," +
                   "comment:" + image.Comment + "," +
                   "liked:" + image.Liked.ToString() + "}";
        }
    }
}
