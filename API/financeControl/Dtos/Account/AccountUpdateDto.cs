using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Account
{
public class AccountUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string? RazaoSocial { get; set; }
        [MaxLength(14)]
        public string? Cnpj { get; set; }

        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public bool? Ativo { get; set; }
    }
}