using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feex.Infrastructure;

namespace Feex.Application.DTO
{
    public class DivisionSummaryPage
    {
        public PaginatedResponse<DivisionResponseDto> PaginatedDivisions { get; set; }
    }

}
