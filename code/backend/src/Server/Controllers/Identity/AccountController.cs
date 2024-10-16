using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Interfaces.Services.Account;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Shared.Wrapper;

namespace budgetApplyApi.Server.Controllers.Identity
{
    [Authorize]
    [Tags("1.3｜account｜帳戶管理")]
    [Route("api/identity/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUser;

        public AccountController(IAccountService accountService, ICurrentUserService currentUser)
        {
            _accountService = accountService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 根據登入身分，取得使用者資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(Result<GetProfileResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProfile()
        {
            var response = await _accountService.GetProfileAsync();
            return Ok(response);
        }
    }
}