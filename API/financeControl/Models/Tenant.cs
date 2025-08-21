using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace financeControl.Models
{
    public class Tenant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        public decimal LimiteAprovacaoAutomatica { get; set; } = 1000;

        public virtual ICollection<UserTenantRole> UserTenantRoles { get; set; } = new List<UserTenantRole>();
        public virtual ICollection<FinancialObligation> FinancialObligations { get; set; } = new List<FinancialObligation>();
        public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
        public virtual ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}