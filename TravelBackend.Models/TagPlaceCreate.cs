using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class TagPlaceCreate
    {
        [Required]
        public Guid PlaceId { get; set; }

        [Required]
        public Guid TagId { get; set; }
    }
}
