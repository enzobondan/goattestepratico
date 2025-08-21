using AutoMapper;
using financeControl.Dtos.Account;
using financeControl.Dtos.User;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAccountRoleRepository _accountRoleRepo;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        public AccountsController(
            IAccountRepository accountRepo,
            IAccountRoleRepository accountRoleRepo,
            IUserRepository userRepo, 
            IMapper mapper,
            IAccountService accountService)
        {
            _accountRepo = accountRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _accountRoleRepo = accountRoleRepo;
            _accountService = accountService;
        }

        [HttpPost("create-with-owner")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountCreatedResponseDto>> CreateAccountWithOwner(
            [FromBody] CreateAccountWithOwnerRequestDto request)
        {
            try
            {
                var account = await _accountService.CreateAccountWithOwnerAsync(
                    request.Account, 
                    request.Owner);
                    
                return Ok(new AccountCreatedResponseDto
                {
                    AccountId = account.Id,
                    Message = "Account e usuário owner criados com sucesso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-account")]
        public async Task<ActionResult<AccountGetDto>> GetMyAccount()
        {
            var currentUserId = GetCurrentUserId();
            var user = await _userRepo.GetByIdAsync(currentUserId);
            
            if (user == null) return NotFound("Usuário não encontrado");
            
            var validationResult = ValidateAccountAccess(user.AccountId);
            if (validationResult != null) return validationResult;
            
            var account = await _accountRepo.GetByIdAsync(user.AccountId);
            if (account == null) return NotFound("Account não encontrada");
            
            return Ok(_mapper.Map<AccountGetDto>(account));
        }

        [HttpPut("my-account")]
        public async Task<ActionResult<AccountGetDto>> UpdateMyAccount([FromBody] AccountUpdateDto updateDto)
        {
            var currentUserId = GetCurrentUserId();
            var user = await _userRepo.GetByIdAsync(currentUserId);

            if (user == null) return NotFound("Usuário não encontrado");

            var account = await _accountRepo.GetByIdAsync(user.AccountId);
            if (account == null) return NotFound("Account não encontrada");

            var hasPermission = await _accountRoleRepo.UserIsAccountAdminAsync(currentUserId, account.Id);
            if(!hasPermission) return Unauthorized("Usuário não é administrador da Account, impossível atualizar");

            _mapper.Map(updateDto, account);
            _accountRepo.Update(account);

            if (await _accountRepo.SaveAllAsync())
            {
                return Ok(_mapper.Map<AccountGetDto>(account));
            }

            return BadRequest("Falha ao atualizar a account");
        }
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<AccountGetDto>> GetById(Guid id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null) return NotFound();

            return Ok(_mapper.Map<AccountGetDto>(account));
        }

        [HttpPost]
        public async Task<ActionResult<AccountGetDto>> Create([FromBody] AccountCreateDto createDto)
        {
            var existingAccount = await _accountRepo.GetByCnpjAsync(createDto.Cnpj);
            if (existingAccount != null)
                return BadRequest("Já existe uma conta com este CNPJ.");

            var account = _mapper.Map<Account>(createDto);
            await _accountRepo.AddAsync(account);

            if (await _accountRepo.SaveAllAsync())
            {
                var accountDto = _mapper.Map<AccountGetDto>(account);
                return CreatedAtAction(nameof(GetById), new { id = account.Id }, accountDto);
            }

            return BadRequest("Falha ao criar a conta.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountGetDto>> Update(Guid id, [FromBody] AccountUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest("ID da URL não corresponde ao ID do corpo.");

            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null) return NotFound();

            _mapper.Map(updateDto, account);
            _accountRepo.Update(account);

            if (await _accountRepo.SaveAllAsync())
                return Ok(_mapper.Map<AccountGetDto>(account));

            return BadRequest("Falha ao atualizar a conta.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null) return NotFound();

            _accountRepo.Delete(account);
            return await _accountRepo.SaveAllAsync() ? NoContent() : BadRequest("Falha ao deletar a conta.");
        }

        public class CreateAccountWithOwnerRequestDto
        {
            public AccountCreateDto Account { get; set; } = null!;
            public UserCreateDto Owner { get; set; } = null!;
        }

        public class AccountCreatedResponseDto
        {
            public Guid AccountId { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }
}