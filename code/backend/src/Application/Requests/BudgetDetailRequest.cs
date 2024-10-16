namespace budgetApplyApi.Application.Requests
{
    public class BudgetDetailRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SubjectCode { get; set; }
    }
}