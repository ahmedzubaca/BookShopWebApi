using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.OrderDTOs
{
    public class OrderSummaryResponseDTO
    {
        public IEnumerable<productsSummaryByIdDTO>? ProductsSummaryDTOs { get; set; }
        public ShippingDetailsDTO? ShippingDetails { get; set; }
    }
}
