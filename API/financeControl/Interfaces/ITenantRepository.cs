using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetByIdAsync(Guid id);
        Task<Tenant?> GetByCnpjAsync(string cnpj);
        Task<IEnumerable<Tenant>> GetTenantsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(Guid userId);
        Task AddAsync(Tenant tenant);
        void Update(Tenant tenant);
        void Delete(Tenant tenant);
        Task<bool> SaveAllAsync();
    }
}