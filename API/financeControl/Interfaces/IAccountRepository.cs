using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Models;

namespace financeControl.Interfaces
{
public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);
        Task<Account?> GetByCnpjAsync(string cnpj);
        Task<IEnumerable<Account>> GetAllAsync();
        Task AddAsync(Account account);
        void Update(Account account);
        void Delete(Account account);
        Task<bool> SaveAllAsync();
    }
}