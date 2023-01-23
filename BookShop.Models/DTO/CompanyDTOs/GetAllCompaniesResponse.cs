using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.CompanyDTOs
{
    public class GetAllCompaniesResponse
    {
        public List<CompanyDTO>? Companies { get; set; }
    }
}
