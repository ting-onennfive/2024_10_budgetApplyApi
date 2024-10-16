using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Shared.Wrapper;
using Moq;

namespace Application.UnitTests.Features.Budgets.Commands.AddEdit
{
    public class EditBudgetCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly Mock<IRepositoryAsync<Budget, int>> _budgetRepositoryMock;
        private readonly EditBudgetCommandHandler _handler;

        public EditBudgetCommandHandlerTests() 
        {
            _unitOfWorkMock = new();
            _budgetServiceMock = new();
            _budgetRepositoryMock = new();
            _handler = new EditBudgetCommandHandler(_unitOfWorkMock.Object, _budgetServiceMock.Object);

            _unitOfWorkMock.Setup(x => x.Repository<Budget>()).Returns(_budgetRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetIsNotExisted()
        {
            // Arrange
            var command = new EditBudgetCommand();
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Budget)null);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(new List<string> { ResponseMessageConstants.NotExistedOrError }, result.Messages);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenBudgetIsUpdated()
        {
            // Arrange
            var command = new EditBudgetCommand();
            var existedBudget = new Budget() { Id = 1 };
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existedBudget);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(existedBudget.Id, result.Data);
        }

        [Fact]
        public async Task Handler_Should_CallUpdateOnRepository_WhenBudgetIsUpdated()
        {
            // Arrange
            var command = new EditBudgetCommand();
            var existedBudget = new Budget() { Id = 1 };
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existedBudget);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(
                x => x.Repository<Budget>().UpdateAsync(It.Is<Budget>(b => b.Id == existedBudget.Id))
                , Times.Once);
        }
    }
}
