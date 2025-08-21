using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using financeControl.Dtos.ExpenseCategory;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.AspNetCore.Authorization;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;

        public ExpenseCategoriesController(
            IExpenseCategoryRepository expenseCategoryRepo, 
            ITenantRepository tenantRepo, 
            IMapper mapper)
        {
            _expenseCategoryRepo = expenseCategoryRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryGetDto>>> GetByTenantId(Guid tenantId)
        {
            var categories = await _expenseCategoryRepo.GetByTenantIdAsync(tenantId);
            return Ok(_mapper.Map<IEnumerable<ExpenseCategoryGetDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseCategoryGetDto>> GetById(Guid id)
        {
            var category = await _expenseCategoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ExpenseCategoryGetDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseCategoryGetDto>> Create([FromBody] ExpenseCategoryCreateDto createDto)
        {
            var tenant = await _tenantRepo.GetByIdAsync(createDto.TenantId);
            if (tenant == null)
            {
                return BadRequest("Tenant não encontrado.");
            }

            var existingCategory = await _expenseCategoryRepo.GetByNomeAsync(createDto.Nome, createDto.TenantId);
            if (existingCategory != null)
            {
                return BadRequest("Já existe uma categoria com este nome para este tenant.");
            }

            var category = _mapper.Map<ExpenseCategory>(createDto);
            await _expenseCategoryRepo.AddAsync(category);

            if (await _expenseCategoryRepo.SaveAllAsync())
            {
                var categoryDto = _mapper.Map<ExpenseCategoryGetDto>(category);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
            }

            return BadRequest("Falha ao criar a categoria.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseCategoryGetDto>> Update(Guid id, [FromBody] ExpenseCategoryUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID do corpo.");
            }

            var category = await _expenseCategoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (category.Nome != updateDto.Nome)
            {
                var existingCategory = await _expenseCategoryRepo.GetByNomeAsync(updateDto.Nome, category.TenantId);
                if (existingCategory != null && existingCategory.Id != id)
                {
                    return BadRequest("Já existe outra categoria com este nome para este tenant.");
                }
            }

            _mapper.Map(updateDto, category);
            _expenseCategoryRepo.Update(category);

            if (await _expenseCategoryRepo.SaveAllAsync())
            {
                return Ok(_mapper.Map<ExpenseCategoryGetDto>(category));
            }

            return BadRequest("Falha ao atualizar a categoria.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _expenseCategoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _expenseCategoryRepo.Delete(category);

            if (await _expenseCategoryRepo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Falha ao deletar a categoria.");
        }
    }
}