using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; }
        [Required(ErrorMessage = " Image cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Image to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Image is limited to 100 characters!!")]
        public string Image { get; set; }
    }
}
