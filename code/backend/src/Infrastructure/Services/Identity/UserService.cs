using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Interfaces.Services.Identity;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Infrastructure.Models.Identity;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using budgetApplyApi.Shared.Constants.Application;

namespace budgetApplyApi.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            IMapper mapper,
            UserManager<BlazorHeroUser> userManager,
            BlazorHeroContext db,
            IStringLocalizer<UserService> localizer,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync(string searchString)
        {
            var getUsersQuery = _userManager.Users.Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(searchString))
                getUsersQuery = getUsersQuery.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString));
            var users = await getUsersQuery.ToListAsync();
            var response = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(response);
        }

        public async Task<IResult<UserResponse>> GetByIdAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => !x.IsDeleted && x.Id == userId);
            var response = new UserResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return await Result<UserResponse>.SuccessAsync(response);
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request)
        {
            // 確認電子郵件是否重複
            var sameEmailUser = await _userManager.FindByEmailAsync(request.Email);
            if (sameEmailUser != null) return await Result<string>.FailAsync("該電子郵件已存在");
            var sameUserNameUser = await _userManager.FindByNameAsync(request.UserName);
            if (sameUserNameUser != null) return await Result<string>.FailAsync("該帳號已存在");

            var newUser = new BlazorHeroUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                IsActive = true,
                EmailConfirmed = true,
                CreatedBy = string.Empty,
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = string.Empty,
                LastModifiedOn = DateTime.UtcNow
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!createUserResult.Succeeded) return await Result<string>.FailAsync(messages: createUserResult.Errors.Select(x => x.Description).ToList());
            return await Result<string>.SuccessAsync(data: newUser.Id);
        }

        public async Task<IResult> UpdateAsync(UpdateUserRequest request)
        {
            var targetUser = await _userManager.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id);
            if (targetUser == null) return await Result.FailAsync(message: ResponseMessageConstants.NotExistedOrError);
            targetUser.FirstName = request.FirstName;
            targetUser.LastName = request.LastName;
            await _userManager.UpdateAsync(targetUser);
            return await Result.SuccessAsync(message: _localizer["儲存成功"]);
        }
    }
}