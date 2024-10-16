using AutoMapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Features.Budgets.Queries.GetById;
using budgetApplyApi.Domain.Entities;

namespace budgetApplyApi.Application.Mappings
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<AddBudgetCommand, Budget>().ReverseMap();
            CreateMap<Budget, GetBudgetByIdResponse>()
                .ForMember(source => source.Details, dest => dest.MapFrom(y => y.BudgetDetails))
                .ReverseMap();
        }
    }
}