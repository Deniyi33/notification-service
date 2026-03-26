using Microsoft.EntityFrameworkCore;

namespace Feex.Core.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; }
        public string? CallBackUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
