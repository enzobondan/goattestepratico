using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.ExpenseCategory
{
    public class ExpenseCategoryUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string? Nome { get; set; }

        public string? Descricao { get; set; }
        public string? CodigoContaContabil { get; set; }
        public bool? Ativo { get; set; }
    }
}