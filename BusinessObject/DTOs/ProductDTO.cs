using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        public string Image { get; set; } = null!;

        public int QuantitySold { get; set; }

        public double Rate { get; set; }

        public bool? Isdelete { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public int ShopId { get; set; }
    }
}
