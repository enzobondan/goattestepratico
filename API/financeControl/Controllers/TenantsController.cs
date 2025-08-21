using AutoMapper;
using financeControl.Dtos.Tenant;
using financeControl.Helpers;
using financeControl.Interfaces;
using financeControl.Models;
using financeControl.Repository;
using financeControl.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : BaseController
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        private readonly IAccountRoleRepository _accountRoleRepo;

        private readonly IUserRepository _userRepo;

        public TenantsController(
            ITenantRepository tenantRepo,
            IAccountRepository accountRepo,
            IAccountRoleRepository accountRoleRepo,
            IUserRepository userRepo,
            IMapper mapper)
        {
            _tenantRepo = tenantRepo;
            _accountRepo = accountRepo;
            _mapper = mapper;
            _accountRoleRepo = accountRoleRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantGetDto>>> GetAll()
        {
            var currentUserId = GetCurrentUserId();
            
            var userTenants = await _tenantRepo.GetTenantsByUserIdAsync(currentUserId);
            return Ok(_mapper.Map<IEnumerable<TenantGetDto>>(userTenants));
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TenantGetDto>>> GetByAccountId(Guid accountId)
        {
            var validationResult = ValidateAccountAccess(accountId);
            if (validationResult != null) return validationResult;
            
            var tenants = await _tenantRepo.GetTenantsByAccountIdAsync(accountId);
            return Ok(_mapper.Map<IEnumerable<TenantGetDto>>(tenants));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantGetDto>> GetById(Guid id)
        {
            var hasAccess = await HttpContext.UserHasAccessToTenant(id, _tenantRepo);
            if (!hasAccess) return Unauthorized("Usuário não possui acesso ao Tenant");
            var tenant = await _tenantRepo.GetByIdAsync(id);
            return tenant == null ? NotFound() : Ok(_mapper.Map<TenantGetDto>(tenant));
        }

        [HttpPost]
        public async Task<ActionResult<TenantGetDto>> Create([FromBody] TenantCreateDto createDto)
        {
            var userId = GetCurrentUserId();

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return Unauthorized("Usuário não autenticado.");

            var account = await _accountRepo.GetByIdAsync(user.AccountId);
            if (account == null) return BadRequest("Account não encontrada.");

            var hasPermission = await _accountRoleRepo.UserIsAccountAdminAsync(userId, account.Id);
            if (!hasPermission) return Unauthorized("Usuário não é administrador da Account");

            var existingTenant = await _tenantRepo.GetByCnpjAsync(createDto.Cnpj);
            if (existingTenant != null) return BadRequest("Já existe um tenant com este CNPJ.");

            var tenant = _mapper.Map<Tenant>(createDto);
            tenant.AccountId = account.Id;
            await _tenantRepo.AddAsync(tenant);

            return await _tenantRepo.SaveAllAsync()
                ? CreatedAtAction(nameof(GetById), new { id = tenant.Id }, _mapper.Map<TenantGetDto>(tenant))
                : BadRequest("Falha ao criar o tenant.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TenantGetDto>> Update(Guid id, [FromBody] TenantUpdateDto updateDto)
        {
            if (id != updateDto.Id) return BadRequest("ID da URL não corresponde ao ID do corpo.");

            var tenant = await _tenantRepo.GetByIdAsync(id);
            if (tenant == null) return NotFound();

            _mapper.Map(updateDto, tenant);
            _tenantRepo.Update(tenant);

            return await _tenantRepo.SaveAllAsync() 
                ? Ok(_mapper.Map<TenantGetDto>(tenant))
                : BadRequest("Falha ao atualizar o tenant.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tenant = await _tenantRepo.GetByIdAsync(id);
            if (tenant == null) return NotFound();

            _tenantRepo.Delete(tenant);
            return await _tenantRepo.SaveAllAsync() ? NoContent() : BadRequest("Falha ao deletar o tenant.");
        }
    }
}