using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class DescriptionDTO
    {
        [Required(ErrorMessage = "DescriptionId cannot be empty!!")]
        public int DescriptionId { get; set; }
        [Required(ErrorMessage = " Title be empty!!")]
        [MinLength(2, ErrorMessage = " Title to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Title is limited to 100 characters!!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
        [MaxLength(4000, ErrorMessage = "Content is limited to 4000 characters!!")]
        public string Content { get; set; } = null!;
        public List<string> ImageLinks { get; set; }
        public bool? Isdelete { get; set; }
    }

    public class DescriptionCreateDTO
    {
        
        [Required(ErrorMessage = " Title be empty!!")]
        [MinLength(2, ErrorMessage = " Title to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Title is limited to 100 characters!!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
        [MaxLength(4000, ErrorMessage = "Content is limited to 4000 characters!!")]
        public string Content { get; set; } = null!;
        public List<string> ImageLinks { get; set; }


    }
}
