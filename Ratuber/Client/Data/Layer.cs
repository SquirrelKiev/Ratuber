using System;
using System.Collections.Generic;
using ImGuiNET;
using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ratuber.Client.Data.Rules;

namespace Ratuber.Client.Data
{
    [Serializable]
    public class Layer : UniqueObject
    {
        public List<Frame> frames = new();

        public CompositeRule rules = new();

        private GraphicsDevice device;
        private ImGuiRenderer imGuiRenderer;

        public Layer Initialize(GraphicsDevice device, ImGuiRenderer imGuiRenderer)
        {
            this.device = device;
            this.imGuiRenderer = imGuiRenderer;

            foreach(var frame in frames)
            {
                frame.Initialize(device, imGuiRenderer);

                frame.ReloadTexture();
            }

            return this;
        }

        public bool ShouldRender()
        {
            return RuleEvaluator.EvaluateCompositeRule(rules);
        }

        public void Update(GameTime gameTime)
        {

        }

        // TODO: Animation
        public Texture2D GetNextTexture()
        {
            if(frames.Count == 0)
                return null;

            return frames[0].Texture;
        }

        public void RenderLayerEditor()
        {
            RenderLayerEditor(this);
        }

        private static void RenderLayerEditor(Layer layer)
        {
            ImGui.BeginGroup();

            if (ImGui.TreeNode($"Rules##rules{layer.GetUniqueId()}"))
            {
                RuleEditor.RenderRule(layer.rules);

                ImGui.TreePop();
            }
            if (ImGui.TreeNode($"Animation Frames##frames{layer.GetUniqueId()}"))
            {
                for (int i = 0; i < layer.frames.Count; i++)
                {
                    RenderFrameEditor(layer, i);

                    ImGui.Separator();
                }

                if (ImGui.Button($"Add Frame##{layer.GetUniqueId()}"))
                {
                    layer.frames.Add(new Frame().Initialize(layer.device, layer.imGuiRenderer));
                }

                ImGui.TreePop();
            }

            ImGui.EndGroup();
        }

        private static void RenderFrameEditor(Layer layer, int frameIndex)
        {
            var frame = layer.frames[frameIndex];

            ImGui.Text(frameIndex.ToString());

            ImGui.SameLine();

            ImGuiHelpers.MoveElementArrows(layer.frames, frame);

            ImGui.SameLine();

            ImGui.BeginGroup();

            if (ImGui.ImageButton($"Browse##{frame.GetUniqueId()}", ImGuiHelpers.GetSafeTexture(frame.ImGuiTexturePointer), new System.Numerics.Vector2(32, 32)))
            {
                var result = NativeFileDialogSharp.Dialog.FileOpen();

                if (!result.IsError && !result.IsCancelled)
                {
                    frame.texturePath = result.Path;

                    frame.ReloadTexture();
                }
            }

            ImGui.SameLine();

            ImGui.BeginGroup();

            //ImGui.InputText($"##framePath{frame.GetUniqueId()}", ref frame.texturePath, 32767);

            ImGui.SetNextItemWidth(ImGui.GetFontSize() * 12);

            if (frame.isFrameLengthRandom)
            {
                ImGui.InputFloat2($"##frameRandomMinMax", ref frame.randomMinMax);

                frame.randomMinMax = new System.Numerics.Vector2(MathF.Max(frame.randomMinMax.X, 1), MathF.Max(frame.randomMinMax.Y, 1));
            }
            else
            {
                ImGui.InputFloat($"##frameLength{frame.GetUniqueId()}", ref frame.frameLength, 1, 5);

                frame.frameLength = MathF.Max(frame.frameLength, 1);
            }

            ImGui.SameLine();

            ImGui.Checkbox($"##isFrameLengthRandom{frame.GetUniqueId()}", ref frame.isFrameLengthRandom);

            ImGui.SameLine();

            ImGuiHelpers.HelpTooltip(
@"How long the frame will stay on screen in milliseconds. 
When checked, the frame time will be a random number between the first number (min) and the second number (max).
");

            if (ImGui.Button($"Remove Frame##{frame.GetUniqueId()}"))
            {
                layer.frames.RemoveAt(frameIndex);
            }

            ImGui.EndGroup();

            ImGui.EndGroup();
        }
    }
}
