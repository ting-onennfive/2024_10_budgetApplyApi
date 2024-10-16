using AutoMapper;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.AddEdit
{
    public partial class EditBudgetDetailCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 細項描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 預算名稱
        /// </summary>
        public string Name { get; set; }
    }

    internal class EditBudgetDetailCommandHandler : IRequestHandler<EditBudgetDetailCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetService _budgetService;
        private readonly IBudgetDetailService _budgetDetailService;

        public EditBudgetDetailCommandHandler(
            IUnitOfWork<int> unitOfWork
            , IMapper mapper
            , IBudgetService budgetService
            , IBudgetDetailService budgetDetailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _budgetService = budgetService;
            _budgetDetailService = budgetDetailService;
        }

        public async Task<Result<int>> Handle(EditBudgetDetailCommand command, CancellationToken cancellationToken)
        {
            var budgetDetail = await _budgetDetailService.GetByIdAsync(command.Id);
            if (budgetDetail == null) return await Result<int>.FailAsync(ResponseMessageConstants.NotExistedOrError);

            // 確認預算大項是否存在
            var budget = await _budgetService.GetByIdAsync(budgetDetail.BudgetsId);
            if (budget == null) return await Result<int>.FailAsync(ResponseMessageConstants.NotExistedOrError);

            budgetDetail.Name = command.Name;
            budgetDetail.Description = command.Description;
            await _unitOfWork.Repository<BudgetDetail>().UpdateAsync(budgetDetail);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(data: budgetDetail.Id);
        }
    }
}