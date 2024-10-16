using budgetApplyApi.Application.Responses.Categories.BudgetDetails;

namespace budgetApplyApi.Application.Features.Budgets.Queries.GetById
{
    public class GetBudgetByIdResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<BudgetDetailResponse> Details { get; set; }
    }
}