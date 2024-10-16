using System.ComponentModel.DataAnnotations;

namespace budgetApplyApi.Application.Requests.Identity
{
    public class TokenRequest
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Required]
        public string Vcode { get; set; }
    }
}