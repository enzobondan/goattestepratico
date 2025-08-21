using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.ExpenseCategory
{
    public class ExpenseCategoryGetDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? CodigoContaContabil { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}