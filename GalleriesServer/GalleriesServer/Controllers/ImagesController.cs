﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleriesServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalleriesServer.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private int _imageId = 0;
        private IList<BlobItem> _data;
        
        public ImagesController()
        {
            _data = new List<BlobItem>()
            {
                new BlobItem{Id = _imageId++, Comment="First Image", Liked = true, Uri="./../../src/img/index.png" },
                new BlobItem{Id = _imageId++, Comment="Second Image", Liked = true, Uri="./../../src/img/index.png" },
                new BlobItem{Id = _imageId++, Comment="tres bonn Image", Liked = true, Uri="./../../src/img/index.png" }
            };
        }

        [HttpGet]
        public ActionResult GetImages()
        {
            return  Ok( new { results = _data });
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormCollection form)
        {

            var images = _data.Select(x => x).ToList();
            var comment = form["Comment"];

            var filepath = ""; 
            foreach (var file in form.Files)
            {
                if (file.Length > 0)
                {
                    filepath = Path.GetTempPath() + file.FileName;
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    RedirectToAction("PostItem", "Media", new MediaItem { Comment = comment, FileName = file.Name, ImageUri = "Some URI" });

                    //images.Add(new Image() { Id = _imageId++, Comment = comment, Liked = false, Url = filepath + file.Name });
                }
            }
            

            return Ok(new { results = images });
        }


        private string GetImages(IList<BlobItem> images)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            str.Append(images.Select(x => ImageTemplate(x))
                .Aggregate((curr, next) => curr + "," + next) + ",]"
                );
            return str.ToString();
        }

        private string ImageTemplate(BlobItem image)
        {
            return "{id:" + image.Id.ToString() + "," +
                   "url:" + image.Uri + "," +
                   "comment:" + image.Comment + "," +
                   "liked:" + image.Liked.ToString() + "}";
        }
    }
}
