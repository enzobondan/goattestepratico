using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IFinancialObligationRepository
    {
        Task<FinancialObligation?> GetByIdAsync(Guid id);
        Task<IEnumerable<FinancialObligation>> GetByTenantIdAsync(Guid tenantId);
        Task<IEnumerable<FinancialObligation>> GetPendingApprovalAsync(Guid tenantId);
        Task<IEnumerable<FinancialObligation>> GetScheduledPaymentsAsync(DateTime dataPagamento);
        Task<IEnumerable<FinancialObligation>> GetPaidInPeriodAsync(Guid tenantId, DateTime startDate, DateTime endDate);
        
        Task AddAsync(FinancialObligation financialObligation);
        void Update(FinancialObligation financialObligation);
        void Delete(FinancialObligation financialObligation);
        Task<bool> SaveAllAsync();
    }
}