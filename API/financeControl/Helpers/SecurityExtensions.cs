using financeControl.Interfaces;
using financeControl.Models;

namespace financeControl.Helpers
{
    public static class SecurityExtensions
    {
        public static bool UserHasAccessToAccount(this Microsoft.AspNetCore.Http.HttpContext context, Guid accountId)
        {
            var user = context.Items["User"] as User;
            return user != null && user.AccountId == accountId;
        }
        
        public static async Task<bool> UserHasAccessToTenant(this Microsoft.AspNetCore.Http.HttpContext context,
            Guid tenantId, ITenantRepository tenantRepo)
        {
            var user = context.Items["User"] as User;
            if (user == null) return false;
            
            var tenant = await tenantRepo.GetByIdAsync(tenantId);
            if (tenant == null) return false;
            
            if (tenant.AccountId != user.AccountId) return false;
            
            return user.UserTenantRoles.Any(utr => utr.TenantId == tenantId);
        }
    }
}