using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.Delete
{
    public class DeleteBudgetDetailCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
    }

    internal class DeleteBudgetDetailCommandHandler : IRequestHandler<DeleteBudgetDetailCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetDetailService _budgetDetailService;

        public DeleteBudgetDetailCommandHandler(IUnitOfWork<int> unitOfWork
            , IBudgetDetailService budgetDetailService)
        {
            _unitOfWork = unitOfWork;
            _budgetDetailService = budgetDetailService;
        }

        public async Task<Result<int>> Handle(DeleteBudgetDetailCommand command, CancellationToken cancellationToken)
        {
            var budgetDetail = await _budgetDetailService.GetByIdAsync(command.Id);

            // 確認預算項目是否存在
            if (budgetDetail == null) return await Result<int>.FailAsync(ResponseMessageConstants.SourceNotExistedOrError("預算細項"));

            budgetDetail.Del = true;
            budgetDetail.DelKey = budgetDetail.Id;
            await _unitOfWork.Repository<BudgetDetail>().UpdateAsync(budgetDetail);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(budgetDetail.Id, ResponseMessageConstants.DeleteSuccess);
        }
    }
}