using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using SquirrelTube.Client.Data.Rules;

namespace SquirrelTube.Client.Data
{
    [Serializable]
    public class Layer : UniqueObject
    {
        public List<Frame> frames = new();

        public CompositeRule rules = new();

        private GraphicsDevice device;

        public void Initialize(GraphicsDevice device)
        {

        }

        public bool ShouldRender()
        {
            return RuleEvaluator.EvaluateCompositeRule(rules);
        }

        public Texture2D GetNextTexture()
        {
            if(frames.Count == 0)
                return null;

            return frames[0].texture;
        }

        public void RenderLayerEditor()
        {
            RenderLayerEditor(this);
        }

        private static void RenderLayerEditor(Layer layer)
        {
            if (ImGui.TreeNode($"Rules##rules{layer.GetHashCode()}"))
            {
                RuleEditor.RenderCompositeRule(layer.rules);

                ImGui.TreePop();
            }
            if (ImGui.TreeNode($"Animation Frames##frames{layer.GetHashCode()}"))
            {
                for (int i = 0; i < layer.frames.Count; i++)
                {
                    RenderFrameEditor(layer, i);

                    if (ImGui.Button("Remove Frame"))
                    {
                        layer.frames.RemoveAt(i);
                    }
                }

                

                if (ImGui.Button("Add Layer"))
                {
                    layer.frames.Add(new Frame());
                }

                ImGui.TreePop();
            }
        }

        private static void RenderFrameEditor(Layer layer, int frameIndex)
        {
            var frame = layer.frames[frameIndex];
        }
    }
}
