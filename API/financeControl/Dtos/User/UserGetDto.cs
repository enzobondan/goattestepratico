using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Dtos.UserTenantRole;

namespace financeControl.Dtos.User
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }

        public List<UserTenantRoleGetDto> Permissoes { get; set; } = new List<UserTenantRoleGetDto>();
    }
}