using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.CostCenter
{
public class CostCenterGetDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}