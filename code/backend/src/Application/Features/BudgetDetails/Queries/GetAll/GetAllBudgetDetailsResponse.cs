namespace budgetApplyApi.Application.Features.BudgetDetails.Queries.GetAll
{
    public class GetAllBudgetDetailsResponse
    {
        /// <summary>
        /// 預算細項 Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 預算細項代碼
        /// </summary>
        public string DetailCode { get; set;} 
        /// <summary>
        /// 預算細項名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 預算大項 Id
        /// </summary>
        public int BudgetsId { get; set; }
        /// <summary>
        /// 預算大項名稱
        /// </summary>
        public string BudgetName { get; set; }
        /// <summary>
        /// 預算大項代碼
        /// </summary>
        public string BudgetCode { get; set; }
    }
}