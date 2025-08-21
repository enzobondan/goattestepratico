using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using financeControl.Data;
using financeControl.Dtos.User;
using financeControl.Interfaces;
using financeControl.Models;
using financeControl.Repository;
using Microsoft.IdentityModel.Tokens;

namespace financeControl.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        private readonly IUserTenantRoleRepository _userTenantRole;
        private readonly IAccountRoleRepository _accountRoleRepository;
        public AuthService(IUserRepository userRepository, IAccountRoleRepository accountRoleRepository, IConfiguration configuration, ApplicationDBContext context, IUserTenantRoleRepository userTenantRole)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _context = context;
            _accountRoleRepository = accountRoleRepository;
            _userTenantRole = userTenantRole;
        }
        public async Task<User> Register(UserCreateDto userCreateDto, Guid accountId, List<Guid> tenantIds)
        {
            if (await _userRepository.UserExists(userCreateDto.Email))
                throw new Exception("Usuário já existe com este email");

            using var hmac = new HMACSHA512();
            var user = new User
            {
                NomeCompleto = userCreateDto.NomeCompleto,
                Email = userCreateDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDto.Password)),
                PasswordSalt = hmac.Key,
                AccountId = accountId,
                DataCriacao = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAllAsync();

            var accountRole = new AccountRole
            {
                UserId = user.Id,
                AccountId = accountId,
                Role = "User",
                DataCriacao = DateTime.UtcNow
            };
            await _accountRoleRepository.AddAsync(accountRole);
            await _userTenantRole.updateUserTenantRole(tenantIds, user.Id, "User");
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> RegisterFirstUser(UserCreateDto userCreateDto, Guid accountId)
        {
            if (await _userRepository.UserExists(userCreateDto.Email))
                throw new Exception("Usuário já existe com este email");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                NomeCompleto = userCreateDto.NomeCompleto,
                Email = userCreateDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCreateDto.Password)),
                PasswordSalt = hmac.Key,
                AccountId = accountId,
                DataCriacao = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAllAsync();

            return user;
        }
        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception("Usuário não encontrado");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    throw new Exception("Senha inválida");
            }
            user.UltimoLogin = DateTime.UtcNow;
            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return GenerateJwtToken(user);
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
                throw new Exception("JWT secret is not configured.");
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.NomeCompleto)
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<bool> ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("Usuário não encontrado");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(currentPassword));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    throw new Exception("Senha atual inválida");
            }

            using var newHmac = new HMACSHA512();
            user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            user.PasswordSalt = newHmac.Key;

            _userRepository.Update(user);
            return await _userRepository.SaveAllAsync();
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return "Email não pertence a nenhum usuário";
            }
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(2);
            
            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();
            return token;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            {
                throw new Exception("Token inválido ou expirado");
            }

            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            user.PasswordSalt = hmac.Key;

            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            _userRepository.Update(user);
            return await _userRepository.SaveAllAsync();
        }
    }
}