using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Application.Interfaces.Services.Identity;
using budgetApplyApi.Application.Requests.Identity;

namespace budgetApplyApi.Server.Controllers.Identity
{
    [Tags("1.0｜帳戶管理")]
    [Route("api/identity/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 取得所有使用者，可根據文字搜尋使用者姓名
        /// </summary>
        /// <param name="searchString">搜尋文字，針對帳戶姓名</param>
        /// <returns>Status 200 OK</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(string searchString)
        {
            var users = await _userService.GetAllAsync(searchString);
            return Ok(users);
        }

        /// <summary>
        /// 根據使用者的 id，取得使用者資訊
        /// </summary>
        /// <param name="id">使用者於 EIP 的 Id</param>
        /// <returns>Status 200 OK</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByEmployeeUidOrId(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// 編輯使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            return Ok(await _userService.UpdateAsync(request));
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            return Ok(await _userService.RegisterAsync(request));
        }
    }
}