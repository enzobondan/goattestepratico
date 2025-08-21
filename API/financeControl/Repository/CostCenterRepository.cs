using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Data;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.EntityFrameworkCore;

namespace financeControl.Repository
{
    public class CostCenterRepository : ICostCenterRepository
    {
        private readonly ApplicationDBContext _context;

        public CostCenterRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CostCenter?> GetByIdAsync(Guid id)
        {
            return await _context.CostCenters
                .Include(cc => cc.Tenant)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public async Task<CostCenter?> GetByCodigoAsync(string codigo, Guid tenantId)
        {
            return await _context.CostCenters
                .FirstOrDefaultAsync(cc => cc.Codigo == codigo && cc.TenantId == tenantId);
        }

        public async Task<IEnumerable<CostCenter>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.CostCenters
                .Where(cc => cc.TenantId == tenantId && cc.Ativo)
                .OrderBy(cc => cc.Codigo)
                .ToListAsync();
        }

        public async Task AddAsync(CostCenter costCenter)
        {
            await _context.CostCenters.AddAsync(costCenter);
        }

        public void Update(CostCenter costCenter)
        {
            _context.CostCenters.Update(costCenter);
        }

        public void Delete(CostCenter costCenter)
        {
            _context.CostCenters.Remove(costCenter);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}