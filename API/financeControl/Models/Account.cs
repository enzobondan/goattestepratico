using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}