using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SquirrelTube.Client.Data.Rules
{
    public class CompositeRuleJsonConverter : JsonConverter<CompositeRule>
    {
        public override CompositeRule ReadJson(JsonReader reader, Type objectType, CompositeRule existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException("Expected StartObject token");
            }

            string compositeRuleAsString = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "compositeRule")
                {
                    reader.Read();
                    compositeRuleAsString = (string)reader.Value;
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            if (compositeRuleAsString == null)
            {
                throw new JsonSerializationException("Missing compositeRule property");
            }

            CompositeRule compositeRule = ParseCompositeRule(compositeRuleAsString);
            return compositeRule;
        }

        public override void WriteJson(JsonWriter writer, CompositeRule value, JsonSerializer serializer)
        {
            string compositeRuleAsString = ConvertCompositeRuleToString(value);
            writer.WriteStartObject();
            writer.WritePropertyName("compositeRule");
            writer.WriteValue(compositeRuleAsString);
            writer.WriteEndObject();
        }

        private string ConvertCompositeRuleToString(CompositeRule compositeRule)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < compositeRule.SubRules.Count; i++)
            {
                Rule subRule = compositeRule.SubRules[i];

                if (subRule is SingleRule rule)
                {
                    sb.Append($"({rule.Condition} {rule.Operator.ToString()} \"{rule.Result}\")");
                }
                else if (subRule is CompositeRule childCompositeRule)
                {
                    sb.Append($"({ConvertCompositeRuleToString(childCompositeRule)})");
                }

                if (i < compositeRule.SubRules.Count - 1)
                {
                    sb.Append($" {compositeRule.LogicGate.ToString()} ");
                }
            }

            return sb.ToString();
        }


        private static CompositeRule ParseCompositeRule(string compositeRuleAsString)
        {
            Stack<CompositeRule> compositeRuleStack = new Stack<CompositeRule>();
            CompositeRule currentCompositeRule = new CompositeRule();

            var tokens = Tokenize(compositeRuleAsString);

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (token == "(")
                {
                    compositeRuleStack.Push(currentCompositeRule);
                    currentCompositeRule = new CompositeRule();
                }
                else if (token == ")")
                {
                    CompositeRule finishedCompositeRule = currentCompositeRule;
                    currentCompositeRule = compositeRuleStack.Pop();
                    currentCompositeRule.AddSubRule(finishedCompositeRule);
                }
                else if (Enum.TryParse(token, out LogicGate logicGate))
                {
                    currentCompositeRule.LogicGate = logicGate;
                }
                else
                {
                    Operator op = Operator.Equal;
                    if (Enum.TryParse(tokens[i + 1], out Operator parsedOperator))
                    {
                        op = parsedOperator;
                    }
                    string result = tokens[i + 2].Trim('"');
                    currentCompositeRule.AddSubRule(new SingleRule { Condition = token, Operator = op, Result = result });
                    i += 2;
                }
            }

            return currentCompositeRule;
        }

        private static List<string> Tokenize(string input)
        {
            var matches = Regex.Matches(input, @"[\w\.]+|[\(\)\>\<\=\!]+|"".*?""");
            List<string> tokens = new List<string>();

            foreach (Match match in matches.Cast<Match>())
            {
                tokens.Add(match.Value);
            }

            return tokens;
        }
    }
}