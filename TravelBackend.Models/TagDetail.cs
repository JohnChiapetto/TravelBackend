using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class TagDetail
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        public string TagName { get; set; }
    }
}
