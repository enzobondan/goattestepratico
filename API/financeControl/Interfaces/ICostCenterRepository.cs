using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface ICostCenterRepository
    {
        Task<CostCenter?> GetByIdAsync(Guid id);
        Task<CostCenter?> GetByCodigoAsync(string codigo, Guid tenantId);
        Task<IEnumerable<CostCenter>> GetByTenantIdAsync(Guid tenantId);
        Task AddAsync(CostCenter costCenter);
        void Update(CostCenter costCenter);
        void Delete(CostCenter costCenter);
        Task<bool> SaveAllAsync();
    }
}