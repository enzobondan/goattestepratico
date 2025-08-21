using financeControl.Data;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.EntityFrameworkCore;

namespace financeControl.Repository
{
    public class FinancialObligationRepository : IFinancialObligationRepository
    {
        private readonly ApplicationDBContext _context;

        public FinancialObligationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<FinancialObligation?> GetByIdAsync(Guid id)
        {
            return await _context.FinancialObligations
                .Include(fo => fo.Vendor)
                .Include(fo => fo.CostCenter)
                .Include(fo => fo.ExpenseCategory)
                .FirstOrDefaultAsync(fo => fo.Id == id);
        }

        public async Task<IEnumerable<FinancialObligation>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.FinancialObligations
                .Include(fo => fo.Vendor)
                .Include(fo => fo.CostCenter)
                .Include(fo => fo.ExpenseCategory)
                .Where(fo => fo.TenantId == tenantId)
                .OrderByDescending(fo => fo.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinancialObligation>> GetPendingApprovalAsync(Guid tenantId)
        {
            return await _context.FinancialObligations
                .Include(fo => fo.Vendor)
                .Where(fo => fo.TenantId == tenantId && fo.Status == "PendenteAprovacao")
                .OrderBy(fo => fo.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinancialObligation>> GetScheduledPaymentsAsync(DateTime dataPagamento)
        {
            return await _context.FinancialObligations
                .Include(fo => fo.Vendor)
                .Include(fo => fo.Tenant)
                .Where(fo => fo.DataPagamento.HasValue && 
                            fo.DataPagamento.Value.Date == dataPagamento.Date &&
                            fo.Status == "Agendado")
                .ToListAsync();
        }

        public async Task<IEnumerable<FinancialObligation>> GetPaidInPeriodAsync(Guid tenantId, DateTime startDate, DateTime endDate)
        {
            return await _context.FinancialObligations
                .Include(fo => fo.Vendor)
                .Include(fo => fo.CostCenter)
                .Where(fo => fo.TenantId == tenantId && 
                            fo.DataPagamento.HasValue &&
                            fo.DataPagamento.Value >= startDate &&
                            fo.DataPagamento.Value <= endDate &&
                            fo.Status == "Pago")
                .OrderByDescending(fo => fo.DataPagamento)
                .ToListAsync();
        }

        public async Task AddAsync(FinancialObligation financialObligation)
        {
            await _context.FinancialObligations.AddAsync(financialObligation);
        }

        public void Update(FinancialObligation financialObligation)
        {
            _context.FinancialObligations.Update(financialObligation);
        }

        public void Delete(FinancialObligation financialObligation)
        {
            _context.FinancialObligations.Remove(financialObligation);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}