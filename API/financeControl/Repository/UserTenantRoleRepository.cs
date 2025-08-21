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
    public class UserTenantRoleRepository : IUserTenantRoleRepository
    {
        private readonly ApplicationDBContext _context;

        public UserTenantRoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserTenantRole userTenantRole)
        {
            await _context.UserTenantRoles.AddAsync(userTenantRole);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(Guid userId)
        {
            return await _context.UserTenantRoles
                .Where(utr => utr.UserId == userId)
                .Include(utr => utr.Tenant)
                .Select(utr => utr.Tenant)
                .Where(t => t.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTenantRole>> updateUserTenantRole(List<Guid> tenantIds, Guid userId, string role)
        {
            List<UserTenantRole> lista = [];

            foreach (var tenantId in tenantIds)
            {
                var userTenantRole = new UserTenantRole
                {
                    UserId = userId,
                    TenantId = tenantId,
                    Role = role
                };

                await AddAsync(userTenantRole);
                await SaveAllAsync();

                lista.Add(userTenantRole);
            };
            return lista;
        }
}
}