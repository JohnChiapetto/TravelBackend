using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Data
{
    public class TagRequest
    {
        [Key]
        public Guid TagRequestId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Please limit the entry to 50 characters.")]
        public string TagRequestName { get; set; }

        [Required]
        public Guid TagRequestPlace { get; set; }

        [Required]
        public DateTimeOffset TagRequestDate { get; set; }

        [Required]
        public Guid TagRequestUserId { get; set; }

        public TagRequest()
        {
            TagRequestId = Guid.NewGuid();
        }
    }
}
