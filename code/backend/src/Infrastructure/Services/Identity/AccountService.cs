using Microsoft.AspNetCore.Identity;
using budgetApplyApi.Application.Interfaces.Services.Account;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Infrastructure.Models.Identity;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Application.Responses.Identity;

namespace budgetApplyApi.Infrastructure.Services.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public AccountService(
            ICurrentUserService currentUserService,
            UserManager<BlazorHeroUser> userManager)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<IResult<GetProfileResponse>> GetProfileAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            var model = new GetProfileResponse
            {
                Email = user?.Email,
                UserName = user?.UserName,
                LastName = user?.LastName,
                FirstName = user?.FirstName
            };
            return await Result<GetProfileResponse>.SuccessAsync(model);
        }
    }
}