using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class PlaceCreate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string PlaceName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(5000)]
        public string PlaceLocation { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32000)]
        public string PlaceDescription { get; set; }

        public TagListItem[] Tags { get; set; }

        public string PlaceImageUrl { get; set; }
    }
}
