using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.StripeDTOs
{
    public class StripeCheckoutOrderResponse
    {
        public string? SessionId { get; set; }
        public string? PublishableKey { get; set; }
    }
}
