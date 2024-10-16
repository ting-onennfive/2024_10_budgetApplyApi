using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Localization;
using budgetApplyApi.Application.Responses.Identity;
using budgetApplyApi.Application.Interfaces.Services.Identity;
using budgetApplyApi.Application.Configurations;
using budgetApplyApi.Application.Requests.Identity;
using budgetApplyApi.Infrastructure.Models.Identity;
using budgetApplyApi.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using budgetApplyApi.Application.Interfaces.Services;
using budgetApplyApi.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;

namespace budgetApplyApi.Infrastructure.Services.Identity
{
    public class IdentityService : ITokenService
    {
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager;
        private readonly AppConfiguration _appConfig;
        private readonly SignInManager<BlazorHeroUser> _signInManager;
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BlazorHeroContext _db;
        private readonly ICaptchaService _captchaService;
        private readonly CaptchaConfiguration _captcha;
        private readonly IHostingEnvironment _environment;

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public IdentityService(
            BlazorHeroContext db,
            ICaptchaService captchaService,
            UserManager<BlazorHeroUser> userManager, 
            RoleManager<BlazorHeroRole> roleManager,
            IOptions<AppConfiguration> appConfig,
            IOptions<CaptchaConfiguration> captchaConfiguration,
            SignInManager<BlazorHeroUser> signInManager,
            IStringLocalizer<IdentityService> localizer,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment environment)
        {
            _db = db;
            _captchaService = captchaService;
            _userManager = userManager;
            _roleManager = roleManager;
            _appConfig = appConfig.Value;
            _captcha = captchaConfiguration.Value;
            _signInManager = signInManager;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
        {
            #region 檢查驗證碼

            // 若為開發環境，則不檢查驗證碼
            bool isCorrectCaptcha = false;
            if (_environment.IsDevelopment())
            {
                isCorrectCaptcha = true;
            }
            else
            {
                string upperVcode = model.Vcode.ToUpper();
                string sessionValue = _session.GetString(_captcha.Key);
                isCorrectCaptcha = _captchaService.ComputeMd5Hash(upperVcode).Equals(sessionValue);
            }
            if (!isCorrectCaptcha) return await Result<TokenResponse>.FailAsync(message: "驗證碼輸入不正確");

            #endregion

            var user = await _userManager.FindByNameAsync(model.Account);
            if (user == null) return await Result<TokenResponse>.FailAsync(message: "帳號或密碼錯誤");
            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isCorrectPassword) return await Result<TokenResponse>.FailAsync(message: "帳號或密碼錯誤");

            // 登入成功清除驗證碼
            _session.Remove(_captcha.Key);

            //var token = await GenerateJwtAsync(user);
            var response = new TokenResponse()
            {
                Token = await GenerateJwtAsync(user),
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            if (model is null)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);
            }
            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse { Token = token };
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        /// <summary>
        /// 產出 JWT Token
        /// </summary>
        /// <param name="user">登入者資訊</param>
        /// <param name="isWithPermission">是否賦值權限資訊，影響到是否可使用功能</param>
        /// <returns></returns>
        private async Task<string> GenerateJwtAsync(BlazorHeroUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(BlazorHeroUser user)
        {
            // 個人身分資料
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, user.Id),
                new(ClaimTypes.Name, user.LastName)
            };
            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(_localizer["Invalid token"]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}