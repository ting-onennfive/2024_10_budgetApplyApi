using AutoMapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Application.Features.BudgetDetails.Queries.GetById;
using budgetApplyApi.Application.Responses.Categories.BudgetDetails;

namespace budgetApplyApi.Application.Mappings
{
    public class BudgetDetailProfile : Profile
    {
        public BudgetDetailProfile()
        {
            CreateMap<AddBudgetDetailCommand, BudgetDetail>().ReverseMap();
            CreateMap<BudgetDetail, GetBudgetDetailByIdResponse>()
                .ForMember(x => x.BudgetsId, des => des.MapFrom(y => y.Budget.Id))
                .ForMember(x => x.BudgetName, des => des.MapFrom(y => y.Budget.Name))
                .ForMember(x => x.BudgetCode, des => des.MapFrom(y => y.Budget.Code))
                .ForMember(x => x.CompleteCode, des => des.MapFrom(y => $"{y.Budget.Code}{y.DetailCode}"))
                .ReverseMap();
            CreateMap<BudgetDetail, BudgetDetailResponse>()
                .ForMember(des => des.Code, opt => opt.MapFrom(c => c.DetailCode))
                .ReverseMap();
        }
    }
}
