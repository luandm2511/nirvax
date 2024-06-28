using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ImageDTO
    {
        public string LinkImage { get; set; } = null!;

        public int? ProductId { get; set; }

        public int? DescriptionId { get; set; }
    }
}
