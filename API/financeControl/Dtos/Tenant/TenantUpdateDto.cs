using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Tenant
{
public class TenantUpdateDto
    {
        [Required]  
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string? RazaoSocial { get; set; }

        [MaxLength(14)]
        public string? Cnpj { get; set; }

        public decimal? LimiteAprovacaoAutomatica { get; set; }
        public bool? Ativo { get; set; }
    }
}