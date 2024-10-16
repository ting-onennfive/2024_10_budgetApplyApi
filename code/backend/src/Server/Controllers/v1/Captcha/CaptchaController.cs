using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using budgetApplyApi.Application.Configurations;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Shared.Wrapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace budgetApplyApi.Server.Controllers.v1.Captcha
{
    [Tags("1.1｜captcha｜驗證碼")]
    public class CaptchaController : BaseApiController<CaptchaController>
    {
        private readonly ICaptchaService _captchaService;
        private readonly CaptchaConfiguration _captchaConfiguration;

        public CaptchaController(
            ICaptchaService captchaService,
            IOptions<CaptchaConfiguration> captchaConfiguration)
        {
            _captchaService = captchaService;
            _captchaConfiguration = captchaConfiguration.Value;
        }

        private string CaptchaHash
        {
            get
            {
                return HttpContext.Session.GetString(_captchaConfiguration.Key) as string;
            }
            set
            {
                HttpContext.Session.SetString(_captchaConfiguration.Key, value);
            }
        }

        /// <summary>
        /// 取得驗證碼圖片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCaptcha()
        {
            // 隨機產生四個字元
            var randomText = _captchaService.GenerateRandomText(4);
            // 加密後存在 Session，也可以不用加密，比對時一致就好。
            CaptchaHash = _captchaService.ComputeMd5Hash(randomText);

            byte[] generateCaptchaImage = _captchaService.GenerateCaptchaImage(randomText);
            return File(generateCaptchaImage, "image/jpeg");

            /* 用於前端串接使用，回傳 base 64 字元
            var result = Convert.ToBase64String(generateCaptchaImage);
            return Ok(await Result<string>.SuccessAsync(data: result));
            */
        }
    }
}
