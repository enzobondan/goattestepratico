using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Dtos.Account;
using financeControl.Dtos.User;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccountWithOwnerAsync(AccountCreateDto accountDto, UserCreateDto ownerDto);
    }
}