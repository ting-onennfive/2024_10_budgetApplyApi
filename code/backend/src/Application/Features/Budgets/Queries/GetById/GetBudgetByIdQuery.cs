using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using budgetApplyApi.Application.Extensions;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Specifications;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Shared.Constants.Application;

namespace budgetApplyApi.Application.Features.Budgets.Queries.GetById
{
    public class GetBudgetByIdQuery : IRequest<Result<GetBudgetByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, Result<GetBudgetByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBudgetDetailService _budgetDetailService;

        public GetBudgetByIdQueryHandler(
            IUnitOfWork<int> unitOfWork, 
            IMapper mapper, 
            IBudgetDetailService budgetDetailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _budgetDetailService = budgetDetailService;
        }

        public async Task<Result<GetBudgetByIdResponse>> Handle(GetBudgetByIdQuery query, CancellationToken cancellationToken)
        {
            var budgetFilterSpec = new BudgetFilterSpecification(id: query.Id);
            var budget = await _unitOfWork.Repository<Budget>()
                .Entities
                .Include(x => x.BudgetDetails.Where(y => !y.Del))
                .Specify(budgetFilterSpec)
                .FirstOrDefaultAsync();
            if (budget == null) return await Result<GetBudgetByIdResponse>.FailAsync(ResponseMessageConstants.NotExistedOrError);
            var response = _mapper.Map<GetBudgetByIdResponse>(budget);
            return await Result<GetBudgetByIdResponse>.SuccessAsync(response);
        }
    }
}