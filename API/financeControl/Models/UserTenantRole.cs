using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Models
{
    public class UserTenantRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string Role { get; set; } = string.Empty;
        public virtual User User { get; set; } = null!;
        public virtual Tenant Tenant { get; set; } = null!;
        public bool IsValid()
        {
            return User.AccountId == Tenant.AccountId;
        }
    }
}