using BookShop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Domain
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]        
        public int ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [Required]
        public string? ApplicationUserId { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int ProductsCount { get; set; }
    }
}
