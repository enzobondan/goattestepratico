using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using financeControl.Dtos.FinantialObligation;
using financeControl.Interfaces;
using financeControl.Models;

namespace financeControl.Service
{
    public class FinancialObligationService : IFinancialObligationService
    {
        private readonly IMapper _mapper;
        private readonly IFinancialObligationRepository _obligationRepo;
        private readonly IVendorRepository _vendorRepo;

        public FinancialObligationService(IMapper mapper, IFinancialObligationRepository obligationRepo, IVendorRepository vendorRepo)
        {
            _mapper = mapper;
            _obligationRepo = obligationRepo;
            _vendorRepo = vendorRepo;
        }

        public async Task<FinancialObligation> CreateObligationFromDtoAsync(FinancialObligationCreateDto createDto)
        {
            var obligation = _mapper.Map<FinancialObligation>(createDto);

            await _obligationRepo.AddAsync(obligation);
            return obligation;
        }

        public async Task<FinancialObligation> UpdateObligationFromDtoAsync(FinancialObligationUpdateDto updateDto)
        {
            var obligation = await _obligationRepo.GetByIdAsync(updateDto.Id);
            if (obligation == null) throw new KeyNotFoundException("Obrigação financeira não encontrada.");

            _mapper.Map(updateDto, obligation);

            _obligationRepo.Update(obligation);
            return obligation;
        }
    }
        
}