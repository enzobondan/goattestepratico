using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeControl.Dtos.FinantialObligation;
using financeControl.Models;

namespace financeControl.Interfaces
{
    public interface IFinancialObligationService
    {
        Task<FinancialObligation> CreateObligationFromDtoAsync(FinancialObligationCreateDto createDto);
        Task<FinancialObligation> UpdateObligationFromDtoAsync(FinancialObligationUpdateDto updateDto);
    }
}