using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class TagRequestEdit
    {
        [Key]
        public Guid TagRequestId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Please limit the entry to 50 characters.")]
        public string TagRequestName { get; set; }

        public string TagRequestUserId { get; set; }

        public DateTimeOffset TagRequestDate { get; set; }

        public string TagRequestPlace { get; set; }

    }
}
