using AutoMapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Interfaces.Repositories;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Domain.Entities;
using budgetApplyApi.Shared.Constants.Application;
using budgetApplyApi.Shared.Wrapper;
using Moq;

namespace Application.UnitTests.Features.Budgets.Commands.AddEdit
{
    public class AddBudgetCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork<int>> _unitOfWorkMock;
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly Mock<IRepositoryAsync<Budget, int>> _budgetRepositoryMock;
        private readonly AddBudgetCommandHandler _handler;

        public AddBudgetCommandHandlerTests() 
        {
            _mapperMock = new();
            _unitOfWorkMock = new();
            _budgetServiceMock = new();
            _budgetRepositoryMock = new();
            _handler = new AddBudgetCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _budgetServiceMock.Object);

            _unitOfWorkMock.Setup(x => x.Repository<Budget>()).Returns(_budgetRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailResult_WhenCodeIsNotUnique()
        {
            // Arrange
            var command = new AddBudgetCommand { Id = 0 };
            _budgetServiceMock.Setup(x => x.IsUniqueCodeAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
             Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(new List<string> { ResponseMessageConstants.Repeated("預算代碼") }, result.Messages);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenCodeIsUnique()
        {
            // Arrange
            var command = new AddBudgetCommand { Id = 0 };
            _budgetServiceMock.Setup(x => x.IsUniqueCodeAsync(It.IsAny<string>())).ReturnsAsync(true);

            var newBudget = new Budget { Id = 0 };
            var existedBudget = new Budget { Id = 1 };
            _mapperMock.Setup(x => x.Map<Budget>(It.IsAny<AddBudgetCommand>())).Returns(newBudget);
            _unitOfWorkMock.Setup(x => x.Repository<Budget>().AddAsync(newBudget)).Returns(Task.FromResult(existedBudget));

            // Act
            Result<int> result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotEqual(0, result.Data);
        }

        [Fact]
        public async Task Handler_Should_CallAddOnRepository_WhenCodeIsUnique()
        {
            // Arrange
            var command = new AddBudgetCommand { Id = 0 };
            _budgetServiceMock.Setup(x => x.IsUniqueCodeAsync(It.IsAny<string>())).ReturnsAsync(true);

            var newBudget = new Budget { Id = 0 };
            var addedBudget = new Budget();
            _mapperMock.Setup(x => x.Map<Budget>(It.IsAny<AddBudgetCommand>())).Returns(newBudget);
            _unitOfWorkMock.Setup(x => x.Repository<Budget>().AddAsync(It.IsAny<Budget>())).ReturnsAsync(addedBudget);

            // Act
            Result<int> result = await _handler.Handle(command, default);

            // Assert：透過呼叫的次數作為驗證條件，且 id = 0
            _unitOfWorkMock.Verify(
                x => x.Repository<Budget>().AddAsync(It.Is<Budget>(b => b.Id == newBudget.Id))
                , Times.Once);
        }
    }
}
