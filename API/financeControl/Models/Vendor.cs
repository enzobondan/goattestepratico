using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace financeControl.Models
{
    public class Vendor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string CnpjCpf { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Banco { get; set; }
        public string? Agencia { get; set; }
        public string? ContaCorrente { get; set; }
        public string? Pix { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; } = null!;

        public virtual ICollection<FinancialObligation> FinancialObligations { get; set; } = new List<FinancialObligation>();

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}