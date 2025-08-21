using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Vendor
{
   public class VendorGetDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Banco { get; set; }
        public string? Agencia { get; set; }
        public string? ContaCorrente { get; set; }
        public string? Pix { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}