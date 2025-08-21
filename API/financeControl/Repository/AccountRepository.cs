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
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts
                .Include(a => a.Tenants)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account?> GetByCnpjAsync(string cnpj)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Cnpj == cnpj);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts
                .Where(a => a.Ativo)
                .OrderBy(a => a.RazaoSocial)
                .ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public void Update(Account account)
        {
            _context.Accounts.Update(account);
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}