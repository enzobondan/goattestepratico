using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Tenant
{
public class TenantCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } = string.Empty;
        public decimal LimiteAprovacaoAutomatica { get; set; } = 1000;
    }
}