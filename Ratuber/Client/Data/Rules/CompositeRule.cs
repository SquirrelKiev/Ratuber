using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ratuber.Client.Data.Rules
{
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

            ruleType = RuleType.Composite;
        }

        public void AddSubRule(Rule subRule)
        {
            SubRules.Add(subRule);
        }
    }

}
