using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoryTypeEnum
    {
        Income = 1,
        Expense = 2
    }
}
