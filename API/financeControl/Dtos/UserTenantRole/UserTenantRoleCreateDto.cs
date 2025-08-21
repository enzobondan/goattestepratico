using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.UserTenantRole
{
public class UserTenantRoleCreateDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
    }
}