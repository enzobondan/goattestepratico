using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using financeControl.Dtos.CostCenter;
using financeControl.Interfaces;
using financeControl.Models;
using Microsoft.AspNetCore.Authorization;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CostCentersController : ControllerBase
    {
        private readonly ICostCenterRepository _costCenterRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;

        public CostCentersController(
            ICostCenterRepository costCenterRepo, 
            ITenantRepository tenantRepo, 
            IMapper mapper)
        {
            _costCenterRepo = costCenterRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<IEnumerable<CostCenterGetDto>>> GetByTenantId(Guid tenantId)
        {
            var costCenters = await _costCenterRepo.GetByTenantIdAsync(tenantId);
            return Ok(_mapper.Map<IEnumerable<CostCenterGetDto>>(costCenters));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CostCenterGetDto>> GetById(Guid id)
        {
            var costCenter = await _costCenterRepo.GetByIdAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CostCenterGetDto>(costCenter));
        }

        [HttpPost]
        public async Task<ActionResult<CostCenterGetDto>> Create([FromBody] CostCenterCreateDto createDto)
        {
            var tenant = await _tenantRepo.GetByIdAsync(createDto.TenantId);
            if (tenant == null)
            {
                return BadRequest("Tenant não encontrado.");
            }

            var existingCostCenter = await _costCenterRepo.GetByCodigoAsync(createDto.Codigo, createDto.TenantId);
            if (existingCostCenter != null)
            {
                return BadRequest("Já existe um centro de custo com este código para este tenant.");
            }

            var costCenter = _mapper.Map<CostCenter>(createDto);
            await _costCenterRepo.AddAsync(costCenter);

            if (await _costCenterRepo.SaveAllAsync())
            {
                var costCenterDto = _mapper.Map<CostCenterGetDto>(costCenter);
                return CreatedAtAction(nameof(GetById), new { id = costCenter.Id }, costCenterDto);
            }

            return BadRequest("Falha ao criar o centro de custo.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CostCenterGetDto>> Update(Guid id, [FromBody] CostCenterUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID do corpo.");
            }

            var costCenter = await _costCenterRepo.GetByIdAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }

            if (costCenter.Codigo != updateDto.Codigo)
            {
                var existingCostCenter = await _costCenterRepo.GetByCodigoAsync(updateDto.Codigo, costCenter.TenantId);
                if (existingCostCenter != null && existingCostCenter.Id != id)
                {
                    return BadRequest("Já existe outro centro de custo com este código para este tenant.");
                }
            }

            _mapper.Map(updateDto, costCenter);
            _costCenterRepo.Update(costCenter);

            if (await _costCenterRepo.SaveAllAsync())
            {
                return Ok(_mapper.Map<CostCenterGetDto>(costCenter));
            }

            return BadRequest("Falha ao atualizar o centro de custo.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var costCenter = await _costCenterRepo.GetByIdAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }

            _costCenterRepo.Delete(costCenter);

            if (await _costCenterRepo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Falha ao deletar o centro de custo.");
        }
    }
}