using ImGuiNET;
using System;
using System.Linq;

namespace Ratuber.Client.Data.Rules
{
    public static class RuleEditor
    {
        public static void RenderRule(Rule rule)
        {
            ImGui.Separator();

            if (rule is CompositeRule compositeRule)
            {
                RenderCompositeRule(compositeRule);
            }
            else if (rule is SingleRule singleRule)
            {
                RenderSingleRule(singleRule);
            }
        }

        private static void RenderCompositeRule(CompositeRule compositeRule)
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

                if (ImGui.TreeNode($"{(subRule is CompositeRule ? "Composite " : "")}Rule {i}##treeRule{subRule.GetUniqueId()}"))
                {
                    RenderRule(subRule);
                    if (ImGui.Button($"Remove Rule {i}"))
                    {
                        compositeRule.SubRules.RemoveAt(i);
                        break;
                    }
                    ImGui.TreePop();
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
        }

        private static void RenderSingleRule(SingleRule rule)
        {
            string[] conditions = RuleEvaluator.inputs.Keys.ToArray();
            int currentCondition = Array.IndexOf(conditions, rule.Condition);

            float comboWidth = ImGui.GetContentRegionAvail().X / 3;

            ImGui.SetNextItemWidth(comboWidth);
            if (ImGui.Combo($"##condition{rule.GetUniqueId()}", ref currentCondition, conditions, conditions.Length))
            {
                rule.Condition = conditions[currentCondition];
            }

            ImGui.SameLine();

            string[] operators = Enum.GetNames(typeof(Operator));
            int currentOperator = (int)rule.Operator;

            ImGui.SetNextItemWidth(comboWidth);
            if (ImGui.Combo($"##operator{rule.GetUniqueId()}", ref currentOperator, operators, operators.Length))
            {
                rule.Operator = (Operator)currentOperator;
            }

            ImGui.SameLine();

            var result = rule.Result;
            ImGui.SetNextItemWidth(comboWidth);
            ImGui.InputText($"##result{rule.GetUniqueId()}", ref result, 100);
            rule.Result = result;
        }


        private static bool TestRule(Rule rule)
        {
            return RuleEvaluator.EvaluateRule(rule);
        }
    }
}