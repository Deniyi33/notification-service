using EMI.Domain.Entities;
using EMI.Domain.Interfaces;
using EMI.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly AppDbContext _context;

        public EmailRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Email> AddAsync(Email email)
        {
           await _context.Emails.AddAsync(email);
            await _context.SaveChangesAsync();
            return email;

        }

        public async Task UpdateAsync(Email email)
        {
             _context.Emails.Update(email);
            await _context.SaveChangesAsync();
        }
    }
}
