using financeControl.Data;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.EntityFrameworkCore;

namespace financeControl.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDBContext _context;

        public VendorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Vendor?> GetByIdAsync(Guid id)
        {
            return await _context.Vendors
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vendor?> GetByCnpjCpfAsync(string cnpjCpf, Guid tenantId)
        {
            return await _context.Vendors
                .FirstOrDefaultAsync(v => v.CnpjCpf == cnpjCpf && v.TenantId == tenantId);
        }

        public async Task<IEnumerable<Vendor>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.Vendors
                .Where(v => v.TenantId == tenantId && v.Ativo)
                .OrderBy(v => v.Nome)
                .ToListAsync();
        }

        public async Task AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
        }

        public void Update(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
        }

        public void Delete(Vendor vendor)
        {
            _context.Vendors.Remove(vendor);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}