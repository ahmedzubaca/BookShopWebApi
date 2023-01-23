using BookShop.Models.DTO.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ProductDTOs
{
    public class GetSingleProductResponse
    {
        public ProductDTO? Product { get; set; }
    }
}
