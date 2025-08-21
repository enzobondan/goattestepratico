// Service/AccountService.cs - CORRIGIDO
using financeControl.Data;
using financeControl.Dtos.Account;
using financeControl.Dtos.User;
using financeControl.Interfaces;
using financeControl.Models;
using financeControl.Repository;
using Microsoft.EntityFrameworkCore;

namespace financeControl.Service
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDBContext _context;
        private readonly IAuthService _authService;

        private readonly IUserTenantRoleRepository _userTenantRole;

        public AccountService(ApplicationDBContext context, IAuthService authService, IUserTenantRoleRepository userTenantRole)
        {
            _context = context;
            _authService = authService;
            _userTenantRole = userTenantRole;
        }

        public async Task<Account> CreateAccountWithOwnerAsync(AccountCreateDto accountDto, UserCreateDto ownerDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // 1. Criar Account
                var account = new Account
                {
                    RazaoSocial = accountDto.RazaoSocial,
                    Cnpj = accountDto.Cnpj,
                    NomeFantasia = accountDto.NomeFantasia,
                    InscricaoEstadual = accountDto.InscricaoEstadual,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };
                
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();

                // 2. Criar User Owner (usando o AuthService modificado)
                var ownerUser = await _authService.RegisterFirstUser(ownerDto, account.Id);

                var accountRole = new AccountRole
                {
                    UserId = ownerUser.Id,
                    AccountId = account.Id,
                    Role = "Owner",
                    DataCriacao = DateTime.UtcNow
                };
                await _context.AccountRoles.AddAsync(accountRole);
                await _context.SaveChangesAsync();
                
                // 3. Criar Tenant padrão
                var defaultTenant = new Tenant
                {
                    RazaoSocial = account.RazaoSocial,
                    Cnpj = account.Cnpj,
                    AccountId = account.Id,
                    LimiteAprovacaoAutomatica = 1000,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };
                
                await _context.Tenants.AddAsync(defaultTenant);
                await _context.SaveChangesAsync();

                // 4. Criar UserTenantRole para dar acesso ao tenant
                var userTenantRole = new UserTenantRole
                {
                    UserId = ownerUser.Id,
                    TenantId = defaultTenant.Id,
                    Role = "Admin"
                };
                
                await _userTenantRole.AddAsync(userTenantRole);
                await _userTenantRole.SaveAllAsync();

                await transaction.CommitAsync();
                
                return account;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Falha ao criar account com owner: {ex.Message}", ex);
            }
        }
    }
}