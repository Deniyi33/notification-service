using EMI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task<Email> AddAsync(Email email);
        Task UpdateAsync(Email email);
    }
}
