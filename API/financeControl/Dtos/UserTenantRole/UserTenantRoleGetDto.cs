using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.UserTenantRole
{
public class UserTenantRoleGetDto
    {
        public Guid UserId { get; set; }
        public string UserNome { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
        public string TenantRazaoSocial { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DataAssociacao { get; set; }
    }
}