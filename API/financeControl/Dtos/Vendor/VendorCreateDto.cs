using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Vendor
{
public class VendorCreateDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string CnpjCpf { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Banco { get; set; }
        public string? Agencia { get; set; }
        public string? ContaCorrente { get; set; }
        public string? Pix { get; set; }
    }
}