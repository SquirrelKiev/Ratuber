using ImGuiNET;
using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ratuber.Client.Data.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ratuber.Client.Data
{
    public class LayerGroup : UniqueObject
    {
        public string name = string.Empty;

        public CompositeRule rules = new CompositeRule();

        public List<Layer> layers = new List<Layer>();

        private GraphicsDevice device;
        private ImGuiRenderer imGuiRenderer;

        public LayerGroup Initialize(GraphicsDevice device, ImGuiRenderer imGuiRenderer)
        {
            this.device = device;
            this.imGuiRenderer = imGuiRenderer;

            foreach(var layer in layers)
            {
                layer.Initialize(device, imGuiRenderer);
            }

            return this;
        }

        public void Update(GameTime gameTime)
        {
            foreach(var layer in layers)
            {
                layer.Update(gameTime);
            }
        }

        public void RenderLayerGroupEditor()
        {
            RenderLayerGroupEditor(this);
        }

        public static void RenderLayerGroupEditor(LayerGroup layerGroup)
        {
            ImGui.InputText($"Name##{layerGroup.GetUniqueId()}", ref layerGroup.name, 256);

            if (ImGui.TreeNode($"Rules##rules{layerGroup.GetUniqueId()}"))
            {
                RuleEditor.RenderRule(layerGroup.rules);

                ImGui.TreePop();
            }
            if (ImGui.TreeNode($"Layers##layers{layerGroup.GetUniqueId()}"))
            {
                ImGui.Separator();
                for (int i = 0; i < layerGroup.layers.Count; i++)
                {
                    ImGui.Text(i.ToString());

                    ImGui.SameLine();

                    ImGuiHelpers.MoveElementArrows(layerGroup.layers, layerGroup.layers[i]);

                    ImGui.SameLine();

                    layerGroup.layers[i].RenderLayerEditor();

                    if(ImGui.Button($"Remove Layer##{layerGroup.GetUniqueId()}"))
                    {
                        layerGroup.layers.RemoveAt(i);
                    }
                    ImGui.Separator();
                }

                if (ImGui.Button($"Add Layer##{layerGroup.GetUniqueId()}"))
                {
                    layerGroup.layers.Add(new Layer().Initialize(layerGroup.device, layerGroup.imGuiRenderer));
                }

                ImGui.TreePop();
            }
        }

        internal bool ShouldRender()
        {
            return RuleEvaluator.EvaluateCompositeRule(rules);
        }
    }
}
