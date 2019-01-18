using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Models
{
    public class BlobItem
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public string Comment { get; set; }
        public bool Liked { get; set; }
        public string BlobName { get; set; }
    }
}
