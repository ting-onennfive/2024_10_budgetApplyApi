using budgetApplyApi.Application.Interfaces.Common;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Shared.Wrapper;
using System.Threading.Tasks;

namespace budgetApplyApi.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        /// <summary>
        /// 根據帳號密碼，獲取憑證 token
        /// </summary>
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);
        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}