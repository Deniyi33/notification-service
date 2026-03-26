using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Application.DTO
{
    public class DivisionResponseDto
    {
        public long Id { get; set; }
        public string DivisionName { get; set; }
        public long? DivisionalHeadId { get; set; }
        public string? DivisionalHeadName { get; set; }
        public int DepartmentCount { get; set; }
        public int UnitCount { get; set; }
        public int MemberCount { get; set; }
    }
}
