﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs
{
    public class BrandDTO
    {
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; }
        
        public IFormFile ImageFile { get; set; }
        [Required(ErrorMessage = " CategoryId cannot be empty!!")]
        public int CategoryId { get; set; }
    }
}
