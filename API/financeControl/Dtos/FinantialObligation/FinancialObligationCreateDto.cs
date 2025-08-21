using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.FinantialObligation
{
    public class FinancialObligationCreateDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ValorNominal { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public DateTime DataVencimento { get; set; }

        public string? NumeroNotaFiscal { get; set; }
        public string? SerieNotaFiscal { get; set; }
        public string? Status { get; set; }

        public string? FormaPagamento { get; set; }
        public string? ChaveAcessoNFe { get; set; }

        public Guid? CostCenterId { get; set; }
        public Guid? ExpenseCategoryId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorICMS { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorIPI { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorPIS { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorCOFINS { get; set; }
        }
}