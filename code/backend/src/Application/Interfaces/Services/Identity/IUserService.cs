using budgetApplyApi.Application.Interfaces.Common;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Application.Interfaces.Services.Identity
{
    public interface IUserService : IService
    {
        /// <summary>
        /// 取得所有使用者，可根據文字搜尋使用者姓名
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        Task<Result<List<UserResponse>>> GetAllAsync(string searchString);
        /// <summary>
        /// 根據使用者 id，取得使用者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IResult<UserResponse>> GetByIdAsync(string userId);
        /// <summary>
        /// 編輯使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult> UpdateAsync(UpdateUserRequest request);
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<string>> RegisterAsync(RegisterRequest request);
    }
}