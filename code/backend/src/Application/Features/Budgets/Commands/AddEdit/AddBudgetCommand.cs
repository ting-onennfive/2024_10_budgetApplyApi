using AutoMapper;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Wrapper;
using MediatR;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Application.Interfaces.Services;

namespace budgetApplyApi.Application.Features.Budgets.Commands.AddEdit
{
    public partial class AddBudgetCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        /// <summary>
        /// 預算代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 預算名稱
        /// </summary>
        public string Name { get; set; }
    }

    internal class AddBudgetCommandHandler : IRequestHandler<AddBudgetCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBudgetService _budgetService;

        public AddBudgetCommandHandler(
            IUnitOfWork<int> unitOfWork, 
            IMapper mapper,
            IBudgetService budgetService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _budgetService = budgetService;
        }

        public async Task<Result<int>> Handle(AddBudgetCommand command, CancellationToken cancellationToken)
        {
            // 確認預算代碼是否不重複
            if (!await _budgetService.IsUniqueCodeAsync(command.Code))
                return await Result<int>.FailAsync(ResponseMessageConstants.Repeated("預算代碼"));

            var newBudget = _mapper.Map<Budget>(command);
            newBudget = await _unitOfWork.Repository<Budget>().AddAsync(newBudget);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(data: newBudget.Id);
        }
    }
}