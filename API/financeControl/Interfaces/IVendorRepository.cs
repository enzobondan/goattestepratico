using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IVendorRepository
    {
        Task<Vendor?> GetByIdAsync(Guid id);
        Task<Vendor?> GetByCnpjCpfAsync(string cnpjCpf, Guid tenantId);
        Task<IEnumerable<Vendor>> GetByTenantIdAsync(Guid tenantId);
        Task AddAsync(Vendor vendor);
        void Update(Vendor vendor);
        void Delete(Vendor vendor);
        Task<bool> SaveAllAsync();
    }
}
