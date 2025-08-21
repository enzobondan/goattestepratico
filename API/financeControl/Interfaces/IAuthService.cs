using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Dtos.User;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterFirstUser(UserCreateDto userCreateDto, Guid accountId);
        Task<User> Register(UserCreateDto userCreateDto, Guid accountId, List<Guid> tenantIds);
        Task<string> Login(string email, string password);
        Task<bool> ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task<string> GeneratePasswordResetToken(string email);
        Task<bool> ResetPassword(string token, string newPassword);
    }
}