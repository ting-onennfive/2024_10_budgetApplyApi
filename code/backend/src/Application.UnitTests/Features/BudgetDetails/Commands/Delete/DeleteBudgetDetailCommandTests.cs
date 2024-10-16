using budgetApplyApi.Application.Features.Budgets.Commands.Delete;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Shared.Wrapper;
using Moq;

namespace Application.UnitTests.Features.BudgetDetails.Commands.Delete
{
    public class DeleteBudgetDetailCommandTests
    {
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly Mock<IBudgetDetailService> _budgetDetailServiceMock;
        private readonly Mock<IRepositoryAsync<BudgetDetail, int>> _budgetDetailRepository;
        private readonly DeleteBudgetDetailCommandHandler _handler;

        public DeleteBudgetDetailCommandTests()
        {
            _unitOfWorkMock = new();
            _budgetDetailServiceMock = new();
            _budgetDetailRepository = new();
            _handler = new DeleteBudgetDetailCommandHandler(
                _unitOfWorkMock.Object
                , _budgetDetailServiceMock.Object);

            _unitOfWorkMock.Setup(x => x.Repository<BudgetDetail>()).Returns(_budgetDetailRepository.Object);
            _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetDetailIsNotExisted()
        {
            // Arrange
            var command = new DeleteBudgetDetailCommand();
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BudgetDetail)null);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ResponseMessageConstants.SourceNotExistedOrError("預算細項"), result.Messages.First());
        }

        [Fact]
        public async Task Handler_Shoud_ReturnSuccessResult_WhenBudgetDetailDeletedSuccessfully()
        {
            // Arrange
            var command = new DeleteBudgetDetailCommand();
            var budgetDetail = new BudgetDetail { Id = 1 };
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budgetDetail);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(budgetDetail.Id, result.Data);
        }

        [Fact]
        public async Task Handler_Shoud_CallUpdateOnRepository_WhenBudgetDetailDeletedSuccessfully()
        {
            // Arrange
            var command = new DeleteBudgetDetailCommand();
            var budgetDetail = new BudgetDetail { Id = 1 };
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budgetDetail);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(
                x => x.Repository<BudgetDetail>()
                .UpdateAsync(It.Is<BudgetDetail>(b => b.Id == budgetDetail.Id && b.Del && b.DelKey == budgetDetail.Id))
                , Times.Once);
        }
    }
}
