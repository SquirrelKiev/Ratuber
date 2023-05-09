using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rattuber.Client.Data.Rules
{
    [JsonConverter(typeof(RuleExpressionConverter))]
    public class CompositeRule : Rule
    {
        [JsonProperty("logicGate")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LogicGate LogicGate { get; set; }

        [JsonProperty("subRules")]
        public List<Rule> SubRules { get; private set; }

        public CompositeRule() : base()
        {
            SubRules = new List<Rule>();
        }

        public void AddSubRule(Rule subRule)
        {
            SubRules.Add(subRule);
        }
    }

}
