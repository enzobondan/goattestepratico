using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IExpenseCategoryRepository
    {
        Task<ExpenseCategory?> GetByIdAsync(Guid id);
        Task<ExpenseCategory?> GetByNomeAsync(string nome, Guid tenantId);
        Task<IEnumerable<ExpenseCategory>> GetByTenantIdAsync(Guid tenantId);
        Task AddAsync(ExpenseCategory expenseCategory);
        void Update(ExpenseCategory expenseCategory);
        void Delete(ExpenseCategory expenseCategory);
        Task<bool> SaveAllAsync();
    }
}