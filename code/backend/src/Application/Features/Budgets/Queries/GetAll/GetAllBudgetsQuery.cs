using MediatR;
using Microsoft.EntityFrameworkCore;
using budgetApplyApi.Application.Extensions;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Specifications;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Application.Features.Budgets.Queries.GetAll
{
    public class GetAllBudgetsQuery : IRequest<Result<List<GetAllBudgetsResponse>>> { }

    internal class GetAllBudgetsQueryHandler : IRequestHandler<GetAllBudgetsQuery, Result<List<GetAllBudgetsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllBudgetsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAllBudgetsResponse>>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
        {
            var budgetFilterSpec = new BudgetFilterSpecification();
            var response = await _unitOfWork.Repository<Budget>()
                .Entities
                .Specify(budgetFilterSpec)
                .Include(x => x.BudgetDetails.Where(y => !y.Del & y.Status == 1))
                .Select(x => new GetAllBudgetsResponse()
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    DetailCount = x.BudgetDetails.Count(),
                })
                .OrderBy(x => x.Code)
                .ToListAsync();
            return await Result<List<GetAllBudgetsResponse>>.SuccessAsync(response);
        }
    }
}