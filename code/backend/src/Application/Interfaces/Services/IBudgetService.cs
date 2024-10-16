using budgetApplyApi.Domain.Entities;

namespace budgetApplyApi.Application.Interfaces.Services
{
    public interface IBudgetService
    {
        /// <summary>
        /// 確認預算代碼是否不重複
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> IsUniqueCodeAsync(string code);
        /// <summary>
        /// 根據 Id 取得資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Budget> GetByIdAsync(int id);
    }
}
