using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
   public class TagCreate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string TagName { get; set; }
    }
}
