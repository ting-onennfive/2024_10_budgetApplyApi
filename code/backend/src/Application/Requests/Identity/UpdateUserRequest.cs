namespace budgetApplyApi.Application.Requests.Identity
{
    public class UpdateUserRequest
    {
        /// <summary>
        /// 使用者 Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string LastName { get; set; }
    }
}
