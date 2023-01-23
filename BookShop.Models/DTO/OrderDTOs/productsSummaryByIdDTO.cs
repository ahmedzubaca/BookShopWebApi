using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.OrderDTOs
{
    public class productsSummaryByIdDTO
    {
        [Required]
        public string? Title { get; set; }        
        public double Price { get; set; }        
        public int Count { get; set; }
        public double ChosenProductsPrice { get; set; }              
    }
}
