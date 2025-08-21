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
    public class AccountRoleRepository : IAccountRoleRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> UserIsAccountAdminAsync(Guid userId, Guid accountId)
        {
            var role = await _context.AccountRoles
                .FirstOrDefaultAsync(ar => ar.UserId == userId && ar.AccountId == accountId);
            return role != null && (role.Role == "Owner");
        }

        public async Task AddAsync(AccountRole accountRole)
        {
            await _context.AccountRoles.AddAsync(accountRole);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}