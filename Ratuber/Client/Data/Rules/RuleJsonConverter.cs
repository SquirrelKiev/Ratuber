using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;

namespace Ratuber.Client.Data.Rules
{
    public class RuleJsonConverter : CustomCreationConverter<Rule>
    {
        private RuleType currentRuleType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            currentRuleType = jobj["ruleType"].ToObject<RuleType>();
            return base.ReadJson(jobj.CreateReader(), objectType, existingValue, serializer);
        }

        public override Rule Create(Type objectType)
        {
            return currentRuleType switch
            {
                RuleType.Single => new SingleRule(),
                RuleType.Composite => new CompositeRule(),
                _ => throw new NotImplementedException(),
            };
        }

    }

}
