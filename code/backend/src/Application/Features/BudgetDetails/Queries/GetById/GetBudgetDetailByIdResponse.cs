namespace budgetApplyApi.Application.Features.BudgetDetails.Queries.GetById
{
    public class GetBudgetDetailByIdResponse
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 預算細項名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 細項代碼
        /// </summary>
        public string DetailCode { get; set; }
        /// <summary>
        /// 細項描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 預算大項 Id
        /// </summary>
        public string BudgetsId { get; set; }
        /// <summary>
        /// 預算大項代碼
        /// </summary>
        public string BudgetCode { get; set; }
        /// <summary>
        /// 預算大項名稱
        /// </summary>
        public string BudgetName { get; set; }
        /// <summary>
        /// 預算大項與預算細項的代碼
        /// </summary>
        public string CompleteCode { get; set; }
    }
}