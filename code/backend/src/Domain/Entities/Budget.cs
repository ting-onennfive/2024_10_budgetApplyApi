using budgetApplyApi.Domain.Contracts;

namespace budgetApplyApi.Domain.Entities
{
    public class Budget : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public byte Status { get; set; } = 1;
        public bool Del { get; set; }
        public int DelKey { get; set; }
        public virtual ICollection<BudgetDetail> BudgetDetails { get; set; }
    }
}