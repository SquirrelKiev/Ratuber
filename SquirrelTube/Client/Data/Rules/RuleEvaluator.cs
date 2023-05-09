namespace SquirrelTube.Client.Data.Rules
{
    using System;
    using System.Collections.Generic;

    public static class RuleEvaluator
    {
        public static Dictionary<string, string> inputs = new();

        public static void UpdateInputDictionary(bool clear = false)
        {
            if (clear)
                inputs.Clear();

            inputs["Constant 1"] = "1";

            for (int i = 0; i < CurrentState.MicrophoneManagers.Count; i++)
            {
                inputs[$"Microphone {i} Decibels"] = CurrentState.MicrophoneManagers[i].DbVal.ToString();
            }
        }

        public static bool EvaluateRule(Rule ruleBase)
        {
            if (ruleBase is SingleRule rule)
                return EvaluateSingleRule(rule);
            else if (ruleBase is CompositeRule compositeRule)
                return EvaluateCompositeRule(compositeRule);
            else
                throw new NotSupportedException("Unsupported rule type.");
        }

        public static bool EvaluateSingleRule(SingleRule rule)
        {
            if (!inputs.ContainsKey(rule.Condition))
                return false;

            string inputValue = inputs[rule.Condition];
            string ruleResult = rule.Result;

            return rule.Operator switch
            {
                Operator.Equal => inputValue.Equals(ruleResult, StringComparison.InvariantCultureIgnoreCase),
                Operator.NotEqual => !inputValue.Equals(ruleResult, StringComparison.InvariantCultureIgnoreCase),
                Operator.GreaterThan => Compare(inputValue, ruleResult) > 0,
                Operator.LessThan => Compare(inputValue, ruleResult) < 0,
                Operator.GreaterThanOrEqual => Compare(inputValue, ruleResult) >= 0,
                Operator.LessThanOrEqual => Compare(inputValue, ruleResult) <= 0,
                _ => throw new NotSupportedException($"Operator {rule.Operator} is not supported."),
            };
        }

        private static int Compare(string a, string b)
        {
            if (double.TryParse(a, out double aNumber) && double.TryParse(b, out double bNumber))
            {
                return aNumber.CompareTo(bNumber);
            }
            return string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EvaluateCompositeRule(CompositeRule compositeRule)
        {
            bool result = compositeRule.LogicGate == LogicGate.And;

            foreach (Rule subRule in compositeRule.SubRules)
            {
                bool subRuleResult = EvaluateRule(subRule);

                if (compositeRule.LogicGate == LogicGate.And)
                    result = result && subRuleResult;
                else if (compositeRule.LogicGate == LogicGate.Or)
                    result = result || subRuleResult;
            }

            return result;
        }
    }

}
