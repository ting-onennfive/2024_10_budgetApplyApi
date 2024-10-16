using MediatR;
using Microsoft.EntityFrameworkCore;
using budgetApplyApi.Application.Extensions;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Specifications;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Application.Features.BudgetDetails.Queries.GetAll
{
    public class GetAllBudgetDetailsQuery : IRequest<Result<List<GetAllBudgetDetailsResponse>>> 
    { 
        public int BudgetId { get; set; }
    }

    internal class GetAllBudgetDetailsQueryHandler : IRequestHandler<GetAllBudgetDetailsQuery, Result<List<GetAllBudgetDetailsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllBudgetDetailsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAllBudgetDetailsResponse>>> Handle(GetAllBudgetDetailsQuery request, CancellationToken cancellationToken)
        {
            var budgetDetailFilterSpec = new BudgetDetailFilterSpecification();
            var getBudgetDetailQuery = _unitOfWork.Repository<BudgetDetail>()
                .Entities
                .Specify(budgetDetailFilterSpec)
                .Include(x => x.Budget)
                .Select(x => new GetAllBudgetDetailsResponse()
                {
                    DetailCode = x.DetailCode,
                    Id = x.Id,
                    Name = x.Name,
                    BudgetsId = x.BudgetsId,
                    BudgetCode = x.Budget.Code,
                    BudgetName = x.Budget.Name
                });

            // 根據預算大項篩選
            if (request.BudgetId > 0)  
                getBudgetDetailQuery = getBudgetDetailQuery.Where(x => x.BudgetsId == request.BudgetId);
            
            // 進行資料取用與排序
            var response = await getBudgetDetailQuery
                .OrderBy(x => x.BudgetsId)
                .ThenBy(x => x.Id)
                .ToListAsync();

            return await Result<List<GetAllBudgetDetailsResponse>>.SuccessAsync(response);
        }
    }
}