﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class SizeDTO
    {
        [Required(ErrorMessage = " SizeId cannot be empty!!")]
        public int SizeId { get; set; }
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Content is limited to 50 characters!!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }

    public class SizeCreateDTO
    {
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Content is limited to 50 characters!!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }
}
