using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using financeControl.Helpers;
using AutoMapper;
using financeControl.Dtos.FinantialObligation;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.AspNetCore.Authorization;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialObligationsController : ControllerBase
    {
        private readonly IFinancialObligationService _financialObligationService;
        private readonly IFinancialObligationRepository _financialObligationRepo;
        private readonly IVendorRepository _vendorRepo;
        private readonly ICostCenterRepository _costCenterRepo;
        private readonly IExpenseCategoryRepository _expenseCategoryRepo;
        private readonly IMapper _mapper;

        public FinancialObligationsController(
            IFinancialObligationService financialObligationService,
            IFinancialObligationRepository financialObligationRepo,
            IVendorRepository vendorRepo,
            ICostCenterRepository costCenterRepo,
            IExpenseCategoryRepository expenseCategoryRepo,
            IMapper mapper)
        {
            _financialObligationService = financialObligationService;
            _financialObligationRepo = financialObligationRepo;
            _vendorRepo = vendorRepo;
            _costCenterRepo = costCenterRepo;
            _expenseCategoryRepo = expenseCategoryRepo;
            _mapper = mapper;
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<IEnumerable<FinancialObligationGetDto>>> GetByTenantId(Guid tenantId)
        {
            var obligations = await _financialObligationRepo.GetByTenantIdAsync(tenantId);
            var obligationsDto = _mapper.Map<IEnumerable<FinancialObligationGetDto>>(obligations);
            return Ok(obligationsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialObligationGetDto>> GetById(Guid id)
        {
            var obligation = await _financialObligationRepo.GetByIdAsync(id);
            if (obligation == null)
            {
                return NotFound();
            }

            var obligationDto = _mapper.Map<FinancialObligationGetDto>(obligation);
            return Ok(obligationDto);
        }
        [HttpGet("tenant/{tenantId}/pending-approval")]
        public async Task<ActionResult<IEnumerable<FinancialObligationGetDto>>> GetPendingApproval(Guid tenantId)
        {
            var obligations = await _financialObligationRepo.GetPendingApprovalAsync(tenantId);
            var obligationsDto = _mapper.Map<IEnumerable<FinancialObligationGetDto>>(obligations);
            return Ok(obligationsDto);
        }

        [HttpPost]
        public async Task<ActionResult<FinancialObligationGetDto>> Create([FromBody] FinancialObligationCreateDto createDto)
        {
            Console.WriteLine("Create chamado");
            var validationResult = await ValidateCreateDto(createDto);
            if (validationResult != null)
            {
                Console.WriteLine("Validation falhou");
                return BadRequest(validationResult);
            }

            try
            {
                var newObligation = await _financialObligationService.CreateObligationFromDtoAsync(createDto);

                if (await _financialObligationRepo.SaveAllAsync())
                {
                    var obligationToReturn = _mapper.Map<FinancialObligationGetDto>(newObligation);
                    Console.WriteLine("Criação OK");
                    return CreatedAtAction(nameof(GetById), new { id = newObligation.Id }, obligationToReturn);
                }

                Console.WriteLine("Falha ao salvar");
                return BadRequest("Falha ao salvar a obrigação financeira.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                return StatusCode(500, $"Erro interno ao criar obrigação: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<FinancialObligationGetDto>> Update(Guid id, [FromBody] FinancialObligationUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID do corpo da requisição.");
            }

            var existingObligation = await _financialObligationRepo.GetByIdAsync(id);
            if (existingObligation == null)
            {
                return NotFound();
            }

            var validationResult = await ValidateUpdateDto(updateDto, existingObligation.TenantId);
            if (validationResult != null)
            {
                return validationResult;
            }

            try
            {
                var updatedObligation = await _financialObligationService.UpdateObligationFromDtoAsync(updateDto);

                if (await _financialObligationRepo.SaveAllAsync())
                {
                    var obligationToReturn = _mapper.Map<FinancialObligationGetDto>(updatedObligation);
                    return Ok(obligationToReturn);
                }

                return BadRequest("Falha ao atualizar a obrigação financeira.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao atualizar obrigação: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obligation = await _financialObligationRepo.GetByIdAsync(id);
            if (obligation == null)
            {
                return NotFound();
            }

            _financialObligationRepo.Delete(obligation);

            if (await _financialObligationRepo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Falha ao deletar a obrigação financeira.");
        }


        [Authorize(Policy = "ApproverOrAdmin")]
        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<FinancialObligationGetDto>> Approve(Guid id)
        {
            var obligation = await _financialObligationRepo.GetByIdAsync(id);
            if (obligation == null)
            {
                return NotFound();
            }

            var currentUser = GetCurrentUser();

            obligation.Status = "Aprovado";
            obligation.DataAprovacao = DateTime.UtcNow;
            obligation.AprovadoPorUserId = currentUser.Id;

            _financialObligationRepo.Update(obligation);

            if (await _financialObligationRepo.SaveAllAsync())
            {
                var obligationToReturn = _mapper.Map<FinancialObligationGetDto>(obligation);
                return Ok(obligationToReturn);
            }

            return BadRequest("Falha ao aprovar a obrigação financeira.");
        }

private async Task<string?> ValidateCreateDto(FinancialObligationCreateDto dto)
{
    var vendor = await _vendorRepo.GetByIdAsync(dto.VendorId);
    if (vendor == null || vendor.TenantId != dto.TenantId)
        return "Fornecedor inválido ou não pertence ao tenant informado.";

    if (dto.CostCenterId.HasValue)
    {
        var costCenter = await _costCenterRepo.GetByIdAsync(dto.CostCenterId.Value);
        if (costCenter == null || costCenter.TenantId != dto.TenantId)
            return "Centro de custo inválido ou não pertence ao tenant informado.";
    }

    if (dto.ExpenseCategoryId.HasValue)
    {
        var expenseCategory = await _expenseCategoryRepo.GetByIdAsync(dto.ExpenseCategoryId.Value);
        if (expenseCategory == null || expenseCategory.TenantId != dto.TenantId)
            return "Categoria de despesa inválida ou não pertence ao tenant informado.";
    }

    return null; // validação OK
}




        private async Task<ActionResult> ValidateUpdateDto(FinancialObligationUpdateDto dto, Guid currentTenantId)
        {
            if (dto.CostCenterId.HasValue)
            {
                var costCenter = await _costCenterRepo.GetByIdAsync(dto.CostCenterId.Value);
                if (costCenter == null || costCenter.TenantId != currentTenantId)
                {
                    return BadRequest("Centro de custo inválido ou não pertence ao tenant da obrigação.");
                }
            }

            if (dto.ExpenseCategoryId.HasValue)
            {
                var expenseCategory = await _expenseCategoryRepo.GetByIdAsync(dto.ExpenseCategoryId.Value);
                if (expenseCategory == null || expenseCategory.TenantId != currentTenantId)
                {
                    return BadRequest("Categoria de despesa inválida ou não pertence ao tenant da obrigação.");
                }
            }

            return NoContent();
        }
        private Guid GetCurrentUserId()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
                throw new Exception("Usuário não autenticado");

            return user.Id;
        }
        
        private User GetCurrentUser()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
                throw new UnauthorizedAccessException("Usuário não autenticado");
            
            return user;
        }
    }
}