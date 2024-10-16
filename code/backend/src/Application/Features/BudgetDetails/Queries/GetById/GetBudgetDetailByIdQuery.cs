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

namespace budgetApplyApi.Application.Features.BudgetDetails.Queries.GetById
{
    public class GetBudgetDetailByIdQuery : IRequest<Result<GetBudgetDetailByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetBudgetDetailByIdQueryHandler : IRequestHandler<GetBudgetDetailByIdQuery, Result<GetBudgetDetailByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetDetailService _budgetDetailService;

        public GetBudgetDetailByIdQueryHandler(
            IUnitOfWork<int> unitOfWork, 
            IMapper mapper, 
            IBudgetDetailService budgetDetailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _budgetDetailService = budgetDetailService;
        }

        public async Task<Result<GetBudgetDetailByIdResponse>> Handle(GetBudgetDetailByIdQuery query, CancellationToken cancellationToken)
        {
            var budgetDetailFilterSpec = new BudgetDetailFilterSpecification(id: query.Id);
            var budgetDetail = await _unitOfWork.Repository<BudgetDetail>()
                .Entities
                .Include(x => x.Budget)
                .Specify(budgetDetailFilterSpec)
                .FirstOrDefaultAsync();
            if (budgetDetail == null) return await Result<GetBudgetDetailByIdResponse>.FailAsync(ResponseMessageConstants.NotExistedOrError);
            var response = _mapper.Map<GetBudgetDetailByIdResponse>(budgetDetail);
            return await Result<GetBudgetDetailByIdResponse>.SuccessAsync(response);
        }
    }
}