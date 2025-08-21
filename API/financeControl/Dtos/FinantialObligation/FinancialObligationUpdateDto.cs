using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.FinantialObligation
{
    public class FinancialObligationUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string? Descricao { get; set; }

        public decimal? ValorNominal { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public string? SerieNotaFiscal { get; set; }
        public string? ChaveAcessoNFe { get; set; }

        public Guid? CostCenterId { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public string? Status { get; set; }
        public string? FormaPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? ProtocoloPagamento { get; set; }
        
        public decimal? ValorICMS { get; set; }
        public decimal? ValorIPI { get; set; }
        public decimal? ValorPIS { get; set; }
        public decimal? ValorCOFINS { get; set; }
    }
} 