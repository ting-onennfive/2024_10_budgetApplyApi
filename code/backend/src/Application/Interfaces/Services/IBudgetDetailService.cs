using budgetApplyApi.Domain.Entities;

namespace budgetApplyApi.Application.Interfaces.Services
{
    public interface IBudgetDetailService
    {
        /// <summary>
        /// 根據 Id 取得資料
        /// </summary>
        /// <param name="id">預算細項 Id</param>
        /// <returns></returns>
        Task<BudgetDetail> GetByIdAsync(int id);
        /// <summary>
        /// 確認是否為不重複代碼
        /// </summary>
        /// <param name="code">預算細項代碼</param>
        /// <param name="budgetId">預算大項 Id</param>
        /// <returns></returns>
        Task<bool> IsUniqueCodeAsync(string code, int budgetId);
    }
}
