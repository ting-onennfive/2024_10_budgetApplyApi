using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.Delete
{
    public class DeleteBudgetCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// 預算大項 Id
        /// </summary>
        public int Id { get; set; }
    }

    internal class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetService _budgetService;

        public DeleteBudgetCommandHandler(
            IUnitOfWork<int> unitOfWork
            , IBudgetService budgetService)
        {
            _unitOfWork = unitOfWork;
            _budgetService = budgetService;
        }

        public async Task<Result<int>> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetByIdAsync(command.Id);

            // 確認預算項目是否存在
            if (budget == null) return await Result<int>.FailAsync(ResponseMessageConstants.SourceNotExistedOrError("預算項目"));
            // 確認無預算細項存在
            if (budget.BudgetDetails.Any()) return await Result<int>.FailAsync(ResponseMessageConstants.ErrorFromReason("無法刪除，尚有預算細項存在"));

            budget.Del = true;
            budget.DelKey = budget.Id;
            await _unitOfWork.Repository<Budget>().UpdateAsync(budget);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(budget.Id, ResponseMessageConstants.DeleteSuccess);
        }
    }
}