﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class SizeChartDTO
    {
        [Required(ErrorMessage = "SizeChartId cannot be empty!!")]
        public int SizeChartId { get; set; }
        [Required(ErrorMessage = "OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
        [Required(ErrorMessage = " Title be empty!!")]
        [MinLength(1, ErrorMessage = " Title to be at least 1 characters!!")]
        [MaxLength(100, ErrorMessage = "Title is limited to 100 characters!!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(4000, ErrorMessage = "Content is limited to 4000 characters!!")]
        public string Content { get; set; } = null!;
        public List<string> ImageLinks { get; set; }
    }

    public class SizeChartCreateDTO
    {
        [Required(ErrorMessage = "OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
        [Required(ErrorMessage = " Title be empty!!")]
        [MinLength(1, ErrorMessage = " Title to be at least 1 characters!!")]
        [MaxLength(100, ErrorMessage = "Title is limited to 100 characters!!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(4000, ErrorMessage = "Content is limited to 4000 characters!!")]
        public string Content { get; set; } = null!;
        public List<string> ImageLinks { get; set; }
    }
}
