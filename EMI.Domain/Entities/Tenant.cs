using EMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMI.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
        public required string Status { get; set; }
        public string? CallBackUrl { get; set; }
        public required string ApiKey { get; set; }
    }
}
