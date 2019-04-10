using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class TagListItem
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Please limit the entry to 50 characters.")]
        public string TagName { get; set; }
    }
}
