using AutoMapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Shared.Wrapper;
using Moq;

namespace Application.UnitTests.Features.BudgetDetails.Commands.AddEdit
{
    public class EditBudgetDetailCommandTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly Mock<IBudgetDetailService> _budgetDetailServiceMock;
        private readonly Mock<IRepositoryAsync<BudgetDetail, int>> _budgetDetailRepository;
        private readonly EditBudgetDetailCommandHandler _handler;

        public EditBudgetDetailCommandTests()
        {
            _mapperMock = new();
            _unitOfWorkMock = new();
            _budgetServiceMock = new();
            _budgetDetailServiceMock = new();
            _budgetDetailRepository = new();
            _handler = new EditBudgetDetailCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _budgetServiceMock.Object,
                _budgetDetailServiceMock.Object);

            _unitOfWorkMock.Setup(x => x.Repository<BudgetDetail>()).Returns(_budgetDetailRepository.Object);
            _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetDetailIsNotExisted()
        {
            // Arrange
            var command = new EditBudgetDetailCommand();
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BudgetDetail)null);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ResponseMessageConstants.NotExistedOrError, result.Messages.First());
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenBudgetIsNotExisted()
        {
            // Arrange
            var command = new EditBudgetDetailCommand();
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new BudgetDetail());
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Budget)null);

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(ResponseMessageConstants.NotExistedOrError, result.Messages.First());
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenBudgetDetailUpdatedSuccessfully()
        {
            // Arrange
            var command = new EditBudgetDetailCommand();
            var budgetDetail = new BudgetDetail { Id = 1 };
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budgetDetail);
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Budget());

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(budgetDetail.Id, result.Data);
        }

        [Fact]
        public async Task Handler_Should_CallUpdateOnRepository_WhenBudgetDetailUpdatedSuccessfully()
        {
            // Arrange
            var command = new EditBudgetDetailCommand
            {
                Description = "text",
                Name = "text"
            };
            var budgetDetail = new BudgetDetail { Id = 1 };
            _budgetDetailServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(budgetDetail);
            _budgetServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Budget());

            // Action
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(
                x => x.Repository<BudgetDetail>()
                .UpdateAsync(It.Is<BudgetDetail>(
                    b => b.Id == budgetDetail.Id && b.Description == command.Description && b.Name == command.Name)),
                Times.Once);
        }
    }
}
