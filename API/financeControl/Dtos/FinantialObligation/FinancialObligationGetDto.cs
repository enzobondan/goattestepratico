using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.FinantialObligation
{
    public class FinancialObligationGetDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorNominal { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public string? SerieNotaFiscal { get; set; }
        public string? ChaveAcessoNFe { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FormaPagamento { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
        public Guid VendorId { get; set; }
        public string VendorNome { get; set; } = string.Empty;
        public Guid? CostCenterId { get; set; }
        public string? CostCenterCodigo { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public string? ExpenseCategoryNome { get; set; }
        public Guid? AprovadoPorUserId { get; set; }
        public string? AprovadoPorUserName { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? ProtocoloPagamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal? ValorICMS { get; set; }
        public decimal? ValorIPI { get; set; }
        public decimal? ValorPIS { get; set; }
        public decimal? ValorCOFINS { get; set; }
        public decimal ValorTotalImpostos { get; set; }
    }


}