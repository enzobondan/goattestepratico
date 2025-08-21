using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace financeControl.Models
{
    public class FinancialObligation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorNominal { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public DateTime DataVencimento { get; set; }

        public string? NumeroNotaFiscal { get; set; }
        public string? SerieNotaFiscal { get; set; }
        public string? ChaveAcessoNFe { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        public string? FormaPagamento { get; set; } = string.Empty;

        [Required]
        public Guid TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; } = null!;

        [Required]
        public Guid VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; } = null!;

        public Guid? CostCenterId { get; set; }

        [ForeignKey("CostCenterId")]
        public virtual CostCenter? CostCenter { get; set; }

        public Guid? ExpenseCategoryId { get; set; }

        [ForeignKey("ExpenseCategoryId")]
        public virtual ExpenseCategory? ExpenseCategory { get; set; }

        public Guid? AprovadoPorUserId { get; set; }

        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? ProtocoloPagamento { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorICMS { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorIPI { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorPIS { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorCOFINS { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotalImpostos => (ValorICMS ?? 0) + (ValorIPI ?? 0) + (ValorPIS ?? 0) + (ValorCOFINS ?? 0);
    }
}