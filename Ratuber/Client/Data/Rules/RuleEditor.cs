using ImGuiNET;
using System;
using System.Linq;

namespace Ratuber.Client.Data.Rules
{
    public static class RuleEditor
    {
        public static void RenderCompositeRule(CompositeRule compositeRule)
        {
            string[] logicGates = { "And", "Or" };
            int currentLogicGate = (int)compositeRule.LogicGate;

            if (ImGui.Combo($"Logic Gate##logicGate{compositeRule.GetUniqueId()}", ref currentLogicGate, logicGates, logicGates.Length))
            {
                compositeRule.LogicGate = (LogicGate)currentLogicGate;
            }

            ImGui.Text("Rules:");
            for (int i = 0; i < compositeRule.SubRules.Count; i++)
            {
                Rule subRule = compositeRule.SubRules[i];
                if (subRule is SingleRule rule)
                {
                    if (ImGui.TreeNode($"Rule {i}##treeRule{subRule.GetUniqueId()}"))
                    {
                        RenderSingleRule(rule);
                        if (ImGui.Button($"Remove Rule {i}"))
                        {
                            compositeRule.SubRules.RemoveAt(i);
                            break;
                        }
                        ImGui.TreePop();
                    }
                }
                else if (subRule is CompositeRule childCompositeRule)
                {
                    if (ImGui.TreeNode($"Composite Rule {i}##treeComp{subRule.GetUniqueId()}"))
                    {
                        ImGui.Indent();
                        RenderCompositeRule(childCompositeRule);
                        if (ImGui.Button($"Remove Rule {i}"))
                        {
                            compositeRule.SubRules.RemoveAt(i);
                            break;
                        }
                        ImGui.Unindent();
                        ImGui.TreePop();
                    }
                }
            }

            if (ImGui.Button("Add Rule"))
            {
                compositeRule.AddSubRule(new SingleRule());
            }
            ImGui.SameLine();
            if (ImGui.Button("Add Composite Rule"))
            {
                compositeRule.AddSubRule(new CompositeRule());
            }

            ImGui.Text(TestRule(compositeRule).ToString());
        }

        public static void RenderSingleRule(SingleRule rule)
        {
            string[] conditions = RuleEvaluator.inputs.Keys.ToArray();
            int currentCondition = Array.IndexOf(conditions, rule.Condition);

            if (ImGui.Combo($"Condition##condition{rule.GetUniqueId()}", ref currentCondition, conditions, conditions.Length))
            {
                rule.Condition = conditions[currentCondition];
            }

            string[] operators = Enum.GetNames(typeof(Operator));
            int currentOperator = (int)rule.Operator;

            if (ImGui.Combo($"Operator##operator{rule.GetUniqueId()}", ref currentOperator, operators, operators.Length))
            {
                rule.Operator = (Operator)currentOperator;
            }
            var result = rule.Result;
            ImGui.InputText($"Result##result{rule.GetUniqueId()}", ref result, 100);
            rule.Result = result;

            ImGui.Text(TestRule(rule).ToString());
        }

        private static bool TestRule(Rule rule)
        {
            return RuleEvaluator.EvaluateRule(rule);
        }
    }
}