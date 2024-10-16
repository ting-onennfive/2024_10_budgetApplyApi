using budgetApplyApi.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace budgetApplyApi.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}