using System.ComponentModel.DataAnnotations;

namespace GalleriesServer.Models
{
    public class MediaItem
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string FileName { get; set; }
        [Required]
        [MaxLength(256)]
        public string ImageUri { get; set; }
        [MaxLength(2048)]
        public string Comment { get; set; }

        public MediaContainer MediaContainer { get; set; }
    }
}
