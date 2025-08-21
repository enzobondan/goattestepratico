using financeControl.Dtos.User;
using financeControl.Interfaces;
using financeControl.Models;
using financeControl.Repository;
using financeControl.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountRepository _accountRepo;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        public AuthController(IAuthService authService, IUserRepository userRepository, IAccountRepository accountRepo, IAccountRoleRepository accountRoleRepository)
        {
            _authService = authService;
            _accountRoleRepository = accountRoleRepository;
            _accountRepo = accountRepo;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var account = await _accountRepo.GetByIdAsync(currentUserId);
                var isAdmin = await _accountRoleRepository.UserIsAccountAdminAsync(currentUserId, account.Id);
                if (!isAdmin)
                    return Forbid("Apenas administradores (donos de account) podem criar usuários");

                var user = await _authService.Register(request.UserCreateDto, account.Id, request.TenantIds);
                return Ok(new
                {
                    user.Id,
                    user.NomeCompleto,
                    user.Email,
                    user.DataCriacao,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _authService.Login(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpGet("current-user")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userRepository.GetByIdAsync(userId);
                return Ok(new
                {
                    user.Id,
                    user.NomeCompleto,
                    user.Email,
                    user.DataCriacao,
                    accountRole = user.AccountRole.Role,

                });
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _authService.ChangePassword(userId, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (success)
                    return Ok(new { message = "Senha alterada com sucesso" });

                return BadRequest("Falha ao alterar senha");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Guid GetCurrentUserId()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
                throw new Exception("Usuário não autenticado");

            return user.Id;
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var result = await _authService.GeneratePasswordResetToken(forgotPasswordDto.Email);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var success = await _authService.ResetPassword(resetPasswordDto.Token, resetPasswordDto.NewPassword);
                
                if (success)
                    return Ok(new { message = "Senha redefinida com sucesso" });
                
                return BadRequest("Falha ao redefinir senha");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class RegisterRequestDto
        {
            [Required] public UserCreateDto UserCreateDto { get; set; } = null!;
            [Required] public List<Guid> TenantIds { get; set; } = new List<Guid>();
        }

        public class LoginDto
        {
            [Required][EmailAddress] public string Email { get; set; } = string.Empty;
            [Required] public string Password { get; set; } = string.Empty;
        }

        public class ForgotPasswordDto
        {
            [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        }

        public class ResetPasswordDto
        {
            [Required] public string Token { get; set; } = string.Empty;
            [Required][MinLength(6)] public string NewPassword { get; set; } = string.Empty;
        }

        public class ChangePasswordDto
        {
            [Required] public string CurrentPassword { get; set; } = string.Empty;
            [Required][MinLength(6)] public string NewPassword { get; set; } = string.Empty;
        }
    }
}