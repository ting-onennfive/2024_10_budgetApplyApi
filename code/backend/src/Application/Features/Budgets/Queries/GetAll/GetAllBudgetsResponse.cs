namespace budgetApplyApi.Application.Features.Budgets.Queries.GetAll
{
    public class GetAllBudgetsResponse
    {
        public int Id { get; set; }
        public string Code { get; set;} 
        public string Name { get; set; }
        public int DetailCount { get; set; }
    }
}