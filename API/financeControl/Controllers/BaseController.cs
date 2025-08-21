using financeControl.Helpers;
using financeControl.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace financeControl.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult? ValidateAccountAccess(Guid accountId)
        {
            var hasAccess = HttpContext.UserHasAccessToAccount(accountId);
            return hasAccess ? null : Forbid();
        }
        
        protected async Task<ActionResult?> ValidateTenantAccess(Guid tenantId, ITenantRepository tenantRepo)
        {
            var hasAccess = await HttpContext.UserHasAccessToTenant(tenantId, tenantRepo);
            return hasAccess ? null : Forbid();
        }
        
        protected Guid GetCurrentUserId()
        {
            if (HttpContext.Items["User"] is Models.User user)
            {
                return user.Id;
            }
            throw new UnauthorizedAccessException("Usuário não autenticado");
        }
    }
}