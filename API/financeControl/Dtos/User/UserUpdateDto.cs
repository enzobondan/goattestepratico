using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace financeControl.Dtos.User
{
public class UserUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(256)]
        public string? NomeCompleto { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        public string? NewPassword { get; set; }
    }
}