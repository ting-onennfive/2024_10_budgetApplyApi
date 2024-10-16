using System.Text.Json;
using budgetApplyApi.Application.Interfaces.Serialization.Options;

namespace budgetApplyApi.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}