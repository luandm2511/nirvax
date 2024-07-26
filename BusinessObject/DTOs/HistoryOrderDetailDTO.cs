using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class HistoryOrderDetailDTO
    {
        public int OrderId { get; set; }
        public string CodeOrder { get; set; }
        public string ShopName { get; set; }
        public string ShopImage { get; set; }
        public int StatusId {  get; set; }
        public string StatusName { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? VoucherId { get; set; }
        public double? VoucherPrice {  get; set; }
        public double TotalPrice { get; set; }
        public string? Note { get; set; }
        public List<HistoryOrderItemDTO> OrderItems { get; set; }
    }
}
