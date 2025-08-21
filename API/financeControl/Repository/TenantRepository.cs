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
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserTenantRoleRepository _userTenantRoleRepository;

        public TenantRepository(ApplicationDBContext context, IUserTenantRoleRepository userTenantRoleRepository)
        {
            _context = context;
            _userTenantRoleRepository = userTenantRoleRepository;
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _context.Tenants
                .Include(t => t.Account)
                .Include(t => t.Vendors)
                .Include(t => t.CostCenters)
                .Include(t => t.FinancialObligations)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant?> GetByCnpjAsync(string cnpj)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Cnpj == cnpj);
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByAccountIdAsync(Guid accountId)
        {
            return await _context.Tenants
                .Where(t => t.AccountId == accountId && t.Ativo)
                .OrderBy(t => t.RazaoSocial)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(Guid userId)
        {
            return await _userTenantRoleRepository.GetTenantsByUserIdAsync(userId);
        }

        public async Task AddAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
        }

        public void Update(Tenant tenant)
        {
            _context.Tenants.Update(tenant);
        }

        public void Delete(Tenant tenant)
        {
            _context.Tenants.Remove(tenant);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}