using budgetApplyApi.Application.Interfaces.Common;

namespace budgetApplyApi.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
        List<KeyValuePair<string, string>> Claims { get; }
    }
}