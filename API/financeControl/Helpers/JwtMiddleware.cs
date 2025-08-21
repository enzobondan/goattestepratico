using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using financeControl.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace financeControl.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            _logger.LogInformation("JwtMiddleware executando...");

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                _logger.LogInformation($"Token encontrado: {token.Substring(0, Math.Min(20, token.Length))}...");
                await AttachUserToContext(context, userRepository, token);
            }
            else
            {
                _logger.LogWarning("Nenhum token JWT encontrado no header Authorization");
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IUserRepository userRepository, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = _configuration["Jwt:Secret"];
                
                if (string.IsNullOrEmpty(secret))
                {
                    _logger.LogError("JWT Secret não configurado");
                    return;
                }
                
                var key = Encoding.ASCII.GetBytes(secret);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                _logger.LogInformation($"Token válido para usuário ID: {userId}");

                var user = await userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    context.Items["User"] = user;
                    
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.NomeCompleto)
                    };
                    
                    var identity = new ClaimsIdentity(claims, "CustomJwt");
                    context.User = new ClaimsPrincipal(identity);
                    
                    _logger.LogInformation($"Usuário autenticado: {user.Email} - ClaimsPrincipal definido");
                }
                else
                {
                    _logger.LogWarning($"Usuário {userId} não encontrado no banco de dados");
                }
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token JWT expirado");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                _logger.LogWarning("Assinatura do token JWT inválida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar token JWT");
            }
        }
    }
}