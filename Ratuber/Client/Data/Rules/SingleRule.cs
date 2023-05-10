using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ratuber.Client.Data.Rules
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

    public enum RuleType
    {
        Single,
        Composite
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

        public SingleRule() : base()
        {
            ruleType = RuleType.Single;
        }
    }
}
