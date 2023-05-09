using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SquirrelTube.Client.Data.Rules
{
    public class CompositeRule : Rule
    {
        [JsonProperty("logicGate")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LogicGate LogicGate { get; set; }

        [JsonProperty("subRules")]
        public List<Rule> SubRules { get; set; }

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
