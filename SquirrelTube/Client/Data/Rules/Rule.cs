using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace SquirrelTube.Client.Data.Rules
{
    public enum Operator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public enum LogicGate
    {
        And,
        Or,
        Not
    }

    public class SingleRule : Rule
    {
        [JsonProperty("condition")]
        public string Condition { get; set; } = string.Empty;

        [JsonProperty("operator")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Operator Operator { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; } = string.Empty;
    }
}
