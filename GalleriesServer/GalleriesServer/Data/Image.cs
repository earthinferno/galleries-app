using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
        public bool Liked { get; set; }
    }
}
