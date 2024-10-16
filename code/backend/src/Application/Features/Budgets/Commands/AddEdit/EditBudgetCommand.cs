using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.AddEdit
{
    public partial class EditBudgetCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// 預算大項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 預算大項名稱
        /// </summary>
        public string Name { get; set; }
    }

    internal class EditBudgetCommandHandler : IRequestHandler<EditBudgetCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetService _budgetService;

        public EditBudgetCommandHandler(
            IUnitOfWork<int> unitOfWork, 
            IBudgetService budgetService)
        {
            _unitOfWork = unitOfWork;
            _budgetService = budgetService;
        }

        public async Task<Result<int>> Handle(EditBudgetCommand command, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetByIdAsync(command.Id);
            if (budget == null) return await Result<int>.FailAsync(ResponseMessageConstants.NotExistedOrError);

            // 編輯僅能異動名稱
            budget.Name = command.Name;
            await _unitOfWork.Repository<Budget>().UpdateAsync(budget);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(budget.Id);
        }
    }
}