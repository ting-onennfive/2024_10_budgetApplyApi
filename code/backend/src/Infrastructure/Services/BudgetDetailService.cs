using budgetApplyApi.Application.Extensions;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Specifications;
using budgetApplyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace budgetApplyApi.Infrastructure.Services
{
    public class BudgetDetailService : IBudgetDetailService
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public BudgetDetailService(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BudgetDetail> GetByIdAsync(int id)
        {
            var budgetDetailFilterSpec = new BudgetDetailFilterSpecification(id);
            return await _unitOfWork.Repository<BudgetDetail>().Entities
                .Specify(budgetDetailFilterSpec)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUniqueCodeAsync(string code, int budgetId)
        {
            var budgetDetailFilterSpec = new BudgetDetailFilterSpecification(detailCode: code, budgetId: budgetId);
            return !await _unitOfWork.Repository<BudgetDetail>().Entities
                .Specify(budgetDetailFilterSpec)
                .AnyAsync();
        }
    }
}
