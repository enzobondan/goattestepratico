using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.ExpenseCategory
{
    public class ExpenseCategoryCreateDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }
        public string? CodigoContaContabil { get; set; }
    }
}