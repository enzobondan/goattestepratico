using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IAccountRoleRepository
    {
        Task<bool> UserIsAccountAdminAsync(Guid userId, Guid accountId);
        Task AddAsync(AccountRole accountRole);
        Task<bool> SaveAllAsync();
    }
}