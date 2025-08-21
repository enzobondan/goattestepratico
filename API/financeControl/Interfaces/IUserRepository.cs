using System;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExists(string email);
        Task AddAsync(User user);
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<User?> GetByResetTokenAsync(string resetToken);
    }
}