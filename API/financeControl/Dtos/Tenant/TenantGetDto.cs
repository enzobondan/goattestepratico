using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Tenant
{
    public class TenantGetDto
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public string AccountRazaoSocial { get; set; } = string.Empty;
        public decimal LimiteAprovacaoAutomatica { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}