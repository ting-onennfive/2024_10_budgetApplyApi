using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Responses.Categories.BudgetDetails;

namespace budgetApplyApi.Application.Features.Budgets.Queries.GetByCode
{
    public class GetBudgetDetailByDetailCodeQuery : IRequest<Result<BudgetDetailResponse>>
    {
        public string Code { get; set; }
    }

    internal class GetBudgetDetailByDetailCodeQueryHandler : IRequestHandler<GetBudgetDetailByDetailCodeQuery, Result<BudgetDetailResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBudgetDetailService _budgetDetailService;

        public GetBudgetDetailByDetailCodeQueryHandler(
            IUnitOfWork<int> unitOfWork, 
            IMapper mapper, 
            IBudgetDetailService budgetDetailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _budgetDetailService = budgetDetailService;
        }

        public async Task<Result<BudgetDetailResponse>> Handle(GetBudgetDetailByDetailCodeQuery query, CancellationToken cancellationToken)
        {
            
            var budget = await _unitOfWork.Repository<BudgetDetail>()
                .Entities
                .FirstOrDefaultAsync(x => x.DetailCode == query.Code);
            if (budget == null) return await Result<BudgetDetailResponse>.FailAsync(ResponseMessageConstants.NotExistedOrError);
            var response = _mapper.Map<BudgetDetailResponse>(budget);
            return await Result<BudgetDetailResponse>.SuccessAsync(response);
        }
    }
}