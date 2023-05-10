using Newtonsoft.Json;
using System;

namespace Ratuber.Client.Data.Rules
{

    [JsonConverter(typeof(RuleJsonConverter))]
    public abstract class Rule : UniqueObject
    {
        [JsonProperty]
        protected RuleType ruleType;
    }
}
