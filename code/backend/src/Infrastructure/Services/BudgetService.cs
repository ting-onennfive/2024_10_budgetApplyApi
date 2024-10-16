using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Specifications;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace budgetApplyApi.Infrastructure.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public BudgetService(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsUniqueCodeAsync(string code)
        {
            var budgetFilterSpec = new BudgetFilterSpecification(code: code);
            var isCodeExisted = await _unitOfWork.Repository<Budget>()
                .Entities
                .Specify(budgetFilterSpec)
                .AnyAsync();
            return !isCodeExisted;
        }

        public async Task<Budget> GetByIdAsync(int id)
        {
            var budgetFilterSpec = new BudgetFilterSpecification(id: id);
            return await _unitOfWork.Repository<Budget>()
                .Entities
                .Specify(budgetFilterSpec)
                .FirstOrDefaultAsync();
        }
    }
}
