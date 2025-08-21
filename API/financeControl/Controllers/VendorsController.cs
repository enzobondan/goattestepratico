using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using financeControl.Dtos;
using financeControl.Interfaces;
using financeControl.Models;
using financeControl.Dtos.Vendor;
using Microsoft.AspNetCore.Authorization;

namespace financeControl.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;

        public VendorsController(IVendorRepository vendorRepo, ITenantRepository tenantRepo, IMapper mapper)
        {
            _vendorRepo = vendorRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<IEnumerable<VendorGetDto>>> GetByTenantId(Guid tenantId)
        {
            var vendors = await _vendorRepo.GetByTenantIdAsync(tenantId);
            var vendorsDto = _mapper.Map<IEnumerable<VendorGetDto>>(vendors);
            return Ok(vendorsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendorGetDto>> GetById(Guid id)
        {
            var vendor = await _vendorRepo.GetByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VendorGetDto>(vendor));
        }

        [HttpPost]
        public async Task<ActionResult<VendorGetDto>> Create([FromBody] VendorCreateDto createDto)
        {
            var tenant = await _tenantRepo.GetByIdAsync(createDto.TenantId);
            if (tenant == null)
            {
                return BadRequest("Tenant não encontrado.");
            }

            var existingVendor = await _vendorRepo.GetByCnpjCpfAsync(createDto.CnpjCpf, createDto.TenantId);
            if (existingVendor != null)
            {
                return BadRequest("Já existe um fornecedor com este CNPJ/CPF para este tenant.");
            }

            var vendor = _mapper.Map<Vendor>(createDto);
            await _vendorRepo.AddAsync(vendor);

            if (await _vendorRepo.SaveAllAsync())
            {
                var vendorDto = _mapper.Map<VendorGetDto>(vendor);
                return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendorDto);
            }

            return BadRequest("Falha ao criar o fornecedor.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendorGetDto>> Update(Guid id, [FromBody] VendorUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID do corpo.");
            }

            var vendor = await _vendorRepo.GetByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDto, vendor);
            _vendorRepo.Update(vendor);

            if (await _vendorRepo.SaveAllAsync())
            {
                return Ok(_mapper.Map<VendorGetDto>(vendor));
            }

            return BadRequest("Falha ao atualizar o fornecedor.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vendor = await _vendorRepo.GetByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _vendorRepo.Delete(vendor);

            if (await _vendorRepo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Falha ao deletar o fornecedor.");
        }
    }
}