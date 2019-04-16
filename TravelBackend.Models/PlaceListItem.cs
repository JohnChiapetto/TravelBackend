using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;

namespace TravelBackend.Models
{
    public class PlaceListItem {
        [Key]
        public Guid PlaceId { get; set; }

        [Required]
        public string PlaceName { get; set; }

        [Required]
        public string PlaceLocation { get; set; }

        [Required]
        public string PlaceDescription { get; set; }

        public string PlaceImageUrl { get; set; } = null;

        [Required]
        public DateTimeOffset SubmittedUTC { get; set; }
        public DateTimeOffset? ModifiedUTC { get; set; } = null;

        public TagListItem[] Tags { get; set; }

        [Required]
        public Guid SubmittingUserId { get; set; }
        public Guid? ModifyingUserId { get; set; } = null;

        public PlaceListItem() { }
        public PlaceListItem(Place p) : this()
        {
            this.PlaceName = p.PlaceName;
            this.PlaceId = p.PlaceId;
            this.PlaceImageUrl = p.PlaceImageUrl;
            this.PlaceLocation = p.PlaceLocation;
            this.SubmittedUTC = p.SubmittedUTC;
            this.SubmittingUserId = p.SubmittingUserId;
            this.PlaceDescription = p.PlaceDescription;
            this.ModifyingUserId = p.ModifyingUserId;
            this.ModifiedUTC = p.ModifiedUTC;
        }
    }
}
