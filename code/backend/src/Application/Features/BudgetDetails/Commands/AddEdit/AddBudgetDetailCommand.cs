using AutoMapper;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.AddEdit
{
    public partial class AddBudgetDetailCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 預算大項 Id
        /// </summary>
        public int BudgetsId { get; set; }
        /// <summary>
        /// 預算代碼
        /// </summary>
        public string DetailCode { get; set; }
        /// <summary>
        /// 細項描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 預算名稱
        /// </summary>
        public string Name { get; set; }
    }

    internal class AddBudgetDetailCommandHandler : IRequestHandler<AddBudgetDetailCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetService _budgetService;
        private readonly IBudgetDetailService _budgetDetailService;

        public AddBudgetDetailCommandHandler(
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

        public async Task<Result<int>> Handle(AddBudgetDetailCommand command, CancellationToken cancellationToken)
        {
            // 確認預算大項是否存在
            var budget = await _budgetService.GetByIdAsync(command.BudgetsId);
            if (budget == null) return await Result<int>.FailAsync(ResponseMessageConstants.NotExistedOrError);

            // 確認預算代碼是否重複
            bool isUniqueCode = await _budgetDetailService.IsUniqueCodeAsync(command.DetailCode, command.BudgetsId);
            if (!isUniqueCode) return await Result<int>.FailAsync(ResponseMessageConstants.Repeated("預算代碼"));

            var newBudgetDetail = _mapper.Map<BudgetDetail>(command);
            await _unitOfWork.Repository<BudgetDetail>().AddAsync(newBudgetDetail);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(data: newBudgetDetail.Id);
        }
    }
}