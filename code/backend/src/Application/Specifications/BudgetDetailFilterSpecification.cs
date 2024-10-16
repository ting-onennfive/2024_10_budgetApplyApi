using LinqKit;
using budgetApplyApi.Application.Specifications.Base;
using budgetApplyApi.Domain.Entities;

namespace budgetApplyApi.Application.Specifications
{
    public class BudgetDetailFilterSpecification : HeroSpecification<BudgetDetail>
    {
        public BudgetDetailFilterSpecification(
            int? id = null,
            int? budgetId = null,
            string detailCode = "",
            string name = "")
        {
            var predicate = PredicateBuilder.New<BudgetDetail>(true);
            predicate = predicate.And(p => !p.Del);
            predicate = predicate.And(p => p.Status == 1);

            if (id.HasValue)
            {
                predicate = predicate.And(p => p.Id == id);
            }

            if (budgetId.HasValue)
            {
                predicate = predicate.And(p => p.BudgetsId == budgetId);
            }

            if (!string.IsNullOrEmpty(detailCode))
            {
                predicate = predicate.And(p => p.DetailCode == detailCode);
            }

            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(p => p.Name == name);
            }

            Criteria = predicate;
        }
    }
}
