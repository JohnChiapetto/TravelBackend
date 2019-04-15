﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBackend.Models
{
    public class TagRequestCreate
    {

        [Required]
        [MaxLength(50, ErrorMessage = "Please limit the entry to 50 characters.")]
        public string TagRequestName { get; set; }

        public DateTimeOffset TagRequestDate { get; set; }

        public Guid TagRequestPlace { get; set; }
        
    }
}
