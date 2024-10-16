using LinqKit;
using budgetApplyApi.Application.Specifications.Base;
using budgetApplyApi.Domain.Entities;

namespace budgetApplyApi.Application.Specifications
{
    public class BudgetFilterSpecification : HeroSpecification<Budget>
    {
        public BudgetFilterSpecification(
            int? id = null,
            string code = "",
            string name = "")
        {
            var predicate = PredicateBuilder.New<Budget>(true);
            predicate = predicate.And(p => !p.Del);

            if (id != null)
            {
                predicate = predicate.And(p => p.Id == id);
            }

            if (!string.IsNullOrEmpty(code))
            {
                predicate = predicate.And(p => p.Code == code);
            }

            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(p => p.Name == name);
            }

            Criteria = predicate;
        }
    }
}
