using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.CoverTypeDTOs
{
    public class AddRequestCoverTypeDTO
    {
        [Required]
        public string? Name { get; set;}    
    }
}
