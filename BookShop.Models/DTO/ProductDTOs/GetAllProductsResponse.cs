using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ProductDTOs
{
    public class GetAllProductsResponse
    {
        public List<ProductDTO>? Products { get; set; }
    }
}
