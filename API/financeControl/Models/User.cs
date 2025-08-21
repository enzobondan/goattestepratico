using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace financeControl.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(256)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = null!;
        
        [Required]
        public byte[] PasswordSalt { get; set; } = null!;

        [Required]
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;
        public virtual AccountRole? AccountRole { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public virtual ICollection<UserTenantRole> UserTenantRoles { get; set; } = new List<UserTenantRole>();

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }
    }
}