using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class PlaceDetails
    {
        [Key]
        public Guid PlaceId { get; set; }

        [Required]
        public string PlaceName { get; set; }

        [Required]
        public string PlaceLocation { get; set; }

        [Required]
        public string PlaceDescription { get; set; }

        public string PlaceImageUrl { get; set; } = null;

        public TagListItem[] Tags { get; set; }

        [Required]
        public DateTimeOffset SubmittedUTC { get; set; }
        public DateTimeOffset? ModifiedUTC { get; set; } = null;

        [Required]
        public Guid SubmittingUserId { get; set; }
        public Guid? ModifyingUserId { get; set; } = null;
    }
}
