using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Vendor
{
public class VendorUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string? Nome { get; set; }

        [MaxLength(14)]
        public string? CnpjCpf { get; set; }

        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Banco { get; set; }
        public string? Agencia { get; set; }
        public string? ContaCorrente { get; set; }
        public string? Pix { get; set; }
        public bool? Ativo { get; set; }
    }
}