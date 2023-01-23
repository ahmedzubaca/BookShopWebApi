using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.CoverTypeDTOs
{
    public class GetAllCoverTypesResponse
    {
        public IEnumerable<CoverTypeDTO>? CoverTypes { get; set; }
    }
}
