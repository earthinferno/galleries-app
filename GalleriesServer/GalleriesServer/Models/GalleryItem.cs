using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Models
{
    public class GalleryItem
    {
        public MediaItem MediaItem { get; set; }
        public int MediaContainerID { get; set; }
    }
}
