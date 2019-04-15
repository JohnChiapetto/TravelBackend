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
        public string TagRequestPlace { get; set; }

        public DateTimeOffset TagRequestDate { get; set; }

        public Guid TagRequestUserId { get; set; }

        public TagRequest()
        {
            TagRequestId = Guid.NewGuid();
        }
    }
}
