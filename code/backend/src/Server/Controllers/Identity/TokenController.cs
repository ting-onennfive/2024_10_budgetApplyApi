using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Application.Interfaces.Services.Identity;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Shared.Wrapper;
using System.Net;

namespace budgetApplyApi.Server.Controllers.Identity
{
    [Tags("1.2｜token｜身份驗證")]
    [Route("api/identity/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _identityService;

        public TokenController(ITokenService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// 取得憑證 Token
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Result<TokenResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(TokenRequest request)
        {
            return Ok(await _identityService.LoginAsync(request));
        }
    }
}