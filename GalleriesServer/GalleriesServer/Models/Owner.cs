using System.ComponentModel.DataAnnotations;

namespace GalleriesServer.Models
{
    public class Owner
    {
        public int ID { get; set; }
        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        [MaxLength(256)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(256)]
        public string ExternalUserId { get; set; }
        [Required]
        [MaxLength(10)]
        public string ExternalIdentityProvider { get; set; }

    }
}
