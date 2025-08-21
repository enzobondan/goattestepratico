using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Account
{
public class AccountCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
    }
}