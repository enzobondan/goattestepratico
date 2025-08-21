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
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public ExpenseCategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ExpenseCategory?> GetByIdAsync(Guid id)
        {
            return await _context.ExpenseCategories
                .Include(ec => ec.Tenant)
                .FirstOrDefaultAsync(ec => ec.Id == id);
        }

        public async Task<ExpenseCategory?> GetByNomeAsync(string nome, Guid tenantId)
        {
            return await _context.ExpenseCategories
                .FirstOrDefaultAsync(ec => ec.Nome == nome && ec.TenantId == tenantId);
        }

        public async Task<IEnumerable<ExpenseCategory>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.ExpenseCategories
                .Where(ec => ec.TenantId == tenantId && ec.Ativo)
                .OrderBy(ec => ec.Nome)
                .ToListAsync();
        }

        public async Task AddAsync(ExpenseCategory expenseCategory)
        {
            await _context.ExpenseCategories.AddAsync(expenseCategory);
        }

        public void Update(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategories.Update(expenseCategory);
        }

        public void Delete(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategories.Remove(expenseCategory);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}