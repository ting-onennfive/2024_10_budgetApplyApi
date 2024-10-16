namespace budgetApplyApi.Application.Responses.Categories.BudgetDetails
{
    public class BudgetDetailResponse
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 預算細項代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 預算細項名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 預算細項描述
        /// </summary>
        public string Description { get; set; }
    }
}