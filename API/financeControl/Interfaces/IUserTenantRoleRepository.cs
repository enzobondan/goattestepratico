using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IUserTenantRoleRepository
    {
        Task AddAsync(UserTenantRole userTenantRole);
        Task<bool> SaveAllAsync();

        Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(Guid userId);

        Task<IEnumerable<UserTenantRole>> updateUserTenantRole(List<Guid> tenantIds, Guid userId, string role);
    }
}