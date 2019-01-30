using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleriesServer.Models
{
    public class MediaContainer
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        public ICollection<MediaItem> MediaItems { get; set; }
       
        public Owner Owner { get; set; }
    }
}
