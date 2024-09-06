using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description cannot be empty!!")]
        [MinLength(10, ErrorMessage = "Description to be at least 10 characters!!")]
        [MaxLength(700, ErrorMessage = "Description is limited to 700 characters!!")]
        public string Description { get; set; } = null!;
        public List<string> ImageLinks {  get; set; }
            
        [Required(ErrorMessage = " Price cannot be empty!!")]
        [Range(0.01, 99999999999, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = " DescriptionId cannot be empty!!")]
        public int SizeChartId { get; set; }
        [Required(ErrorMessage = " CategoryId cannot be empty!!")]
        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }
}
