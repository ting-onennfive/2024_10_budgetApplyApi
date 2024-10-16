using budgetApplyApi.Application.Interfaces.Common;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        /// <summary>
        /// 根據登入身分，取得使用者資訊
        /// </summary>
        /// <returns></returns>
        Task<IResult<GetProfileResponse>> GetProfileAsync();
    }
}