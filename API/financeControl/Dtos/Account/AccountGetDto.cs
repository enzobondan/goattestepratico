using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.Account
{
public class AccountGetDto
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
    }
}