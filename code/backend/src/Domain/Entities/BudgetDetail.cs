using budgetApplyApi.Domain.Contracts;
using System.Text.Json.Serialization;

namespace budgetApplyApi.Domain.Entities
{
    public class BudgetDetail : AuditableEntity<int>
    {
        public int BudgetsId { get; set; }
        public string DetailCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public byte Status { get; set; } = 1;
        public bool Del { get; set; }
        public int DelKey { get; set; }
        [JsonIgnore]
        public Budget Budget { get; set; }
    }
}