using System.ComponentModel.DataAnnotations;

namespace budgetApplyApi.Application.Requests.Identity
{
    public class RegisterRequest
    {
        /// <summary>
        /// 姓氏
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// 確認用密碼
        /// </summary>
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}