using budgetApplyApi.Application.Features.Budgets.Commands.Delete;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Shared.Wrapper;
using Moq;

namespace Application.UnitTests.Features.Budgets.Commands.Delete
{
    public class DeleteBudgetCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly Mock<IRepositoryAsync<Budget, int>> _budgetRepositoryMock;
        private readonly DeleteBudgetCommandHandler _handler;

        public DeleteBudgetCommandHandlerTests()
        {
            _unitOfWorkMock = new();
            _budgetServiceMock = new();
            _budgetRepositoryMock = new();
            _handler = new DeleteBudgetCommandHandler(_unitOfWorkMock.Object, _budgetServiceMock.Object);

            _unitOfWorkMock.Setup(x => x.Repository<Budget>()).Returns(_budgetRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetIsNotExisted()
        {
            // Arrange
            var command = new DeleteBudgetCommand();
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Budget)null);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(new List<string> { ResponseMessageConstants.SourceNotExistedOrError("預算項目") }, result.Messages);
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetHasDetail()
        {
            // Arrange
            var command = new DeleteBudgetCommand();
            var budget = new Budget
            {
                BudgetDetails = new List<BudgetDetail>
                {
                    new BudgetDetail { }
                }
            };
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budget);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(new List<string> { ResponseMessageConstants.ErrorFromReason("無法刪除，尚有預算細項存在") }, result.Messages);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenBudgetIsExistedAndHasNoDetail()
        {
            // Arrange
            var command = new DeleteBudgetCommand();
            var budget = new Budget
            {
                Id = 1,
                BudgetDetails = new List<BudgetDetail>()
            };
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budget);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(budget.Id, result.Data);
        }

        [Fact]
        public async Task Handler_Should_CallUpdateOnRepository_WhenBudgetIsExistedAndHasNoDetail()
        {
            // Arrange
            var command = new DeleteBudgetCommand();
            var budget = new Budget
            {
                Id = 1,
                BudgetDetails = new List<BudgetDetail>()
            };
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budget);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(
                x => x.Repository<Budget>().UpdateAsync(It.Is<Budget>(b => b.Id == budget.Id && b.Del && b.DelKey == budget.Id)),
                Times.Once);
        }
    }
}
