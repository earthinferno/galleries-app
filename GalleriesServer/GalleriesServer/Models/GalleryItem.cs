using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Models
{
    public class GalleryItem
    {
        public MediaItem Item { get; set; }
        public MediaContainer Gallery { get; set; }
        public Owner User { get; set; }
    }
}
