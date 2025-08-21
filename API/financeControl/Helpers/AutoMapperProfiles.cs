using financeControl.Dtos.FinantialObligation;
using financeControl.Dtos.Account;
using financeControl.Dtos.Tenant;
using financeControl.Dtos.Vendor;
using financeControl.Dtos.User;
using financeControl.Dtos.UserTenantRole;
using financeControl.Models;
using AutoMapper;
using System;
using financeControl.Dtos.CostCenter;
using financeControl.Dtos.ExpenseCategory;

namespace financeControl.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountGetDto>();
            CreateMap<AccountCreateDto, Account>();
            CreateMap<AccountUpdateDto, Account>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Tenant, TenantGetDto>().ForMember(dest => dest.AccountRazaoSocial, opt => opt.MapFrom(src => src.Account.RazaoSocial));
            CreateMap<TenantCreateDto, Tenant>();
            CreateMap<TenantUpdateDto, Tenant>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Vendor, VendorGetDto>();
            CreateMap<VendorCreateDto, Vendor>();
            CreateMap<VendorUpdateDto, Vendor>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserGetDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserTenantRole, UserTenantRoleGetDto>()
                .ForMember(dest => dest.UserNome, opt => opt.MapFrom(src => src.User.NomeCompleto))
                .ForMember(dest => dest.TenantRazaoSocial, opt => opt.MapFrom(src => src.Tenant.RazaoSocial));
            CreateMap<UserTenantRoleCreateDto, UserTenantRole>();

            CreateMap<FinancialObligationCreateDto, FinancialObligation>();
            CreateMap<FinancialObligationUpdateDto, FinancialObligation>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CostCenter, CostCenterGetDto>();
            CreateMap<CostCenterCreateDto, CostCenter>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Tenant, opt => opt.Ignore());
            CreateMap<CostCenterUpdateDto, CostCenter>()
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.Tenant, opt => opt.Ignore());

            CreateMap<ExpenseCategory, ExpenseCategoryGetDto>();
            CreateMap<ExpenseCategoryCreateDto, ExpenseCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Tenant, opt => opt.Ignore());
            CreateMap<ExpenseCategoryUpdateDto, ExpenseCategory>()
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.Tenant, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FinancialObligation, FinancialObligationGetDto>()
                .ForMember(dest => dest.VendorNome, opt => opt.MapFrom(src => src.Vendor.Nome))
                .ForMember(dest => dest.CostCenterCodigo, opt => opt.MapFrom(src => src.CostCenter != null ? src.CostCenter.Codigo : null))
                .ForMember(dest => dest.ExpenseCategoryNome, opt => opt.MapFrom(src => src.ExpenseCategory != null ? src.ExpenseCategory.Nome : null))
                .ForMember(dest => dest.AprovadoPorUserName, opt => opt.MapFrom(src => src.AprovadoPorUserId != null ? src.AprovadoPorUserId : null))
                .ForMember(dest => dest.ValorTotalImpostos, opt => opt.MapFrom(src => src.ValorTotalImpostos));
        }
    }
}