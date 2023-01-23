using System.ComponentModel.DataAnnotations;

namespace BookShop.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime? UpdatedDateTime { get; set; }
    }
}
