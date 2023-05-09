using System;
using System.Collections.Generic;
using ImGuiNET;
using ImGuiNET.MonoGame;
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
        private ImGuiRenderer imGuiRenderer;

        public void Initialize(GraphicsDevice device, ImGuiRenderer imGuiRenderer)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            if (imGuiRenderer is null)
                throw new ArgumentNullException(nameof(imGuiRenderer));

            this.device = device;
            this.imGuiRenderer = imGuiRenderer;

            foreach(var frame in frames)
            {
                frame.Initialize(device, imGuiRenderer);

                frame.ReloadTexture();
            }
        }

        public bool ShouldRender()
        {
            return RuleEvaluator.EvaluateCompositeRule(rules);
        }

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
            if (ImGui.TreeNode($"Rules##rules{layer.GetHashCode()}"))
            {
                RuleEditor.RenderCompositeRule(layer.rules);

                ImGui.TreePop();
            }
            if (ImGui.TreeNode($"Animation Frames##frames{layer.GetHashCode()}"))
            {
                for (int i = 0; i < layer.frames.Count; i++)
                {
                    RenderFrameEditor(layer, layer.frames[i]);

                    if (ImGui.Button($"Remove Frame##{layer.frames[i].GetHashCode()}"))
                    {
                        layer.frames.RemoveAt(i);
                    }

                    ImGui.Separator();
                }

                
                 
                if (ImGui.Button($"Add Frame##{layer.GetHashCode()}"))
                {
                    layer.frames.Add(new Frame().Initialize(layer.device, layer.imGuiRenderer));
                }

                ImGui.TreePop();
            }
        }

        private static void RenderFrameEditor(Layer layer, Frame frame)
        {
            ImGui.BeginGroup();

            ImGui.Image(frame.ImGuiTexturePointer, new System.Numerics.Vector2(32, 32));

            ImGui.SameLine();

            ImGui.BeginGroup();
            ImGui.InputText($"##framePath{frame.GetHashCode()}", ref frame.texturePath, 32767);

            if (ImGui.Button($"Browse##{frame.GetHashCode()}"))
            {
                var result = NativeFileDialogSharp.Dialog.FileOpen();

                if (!result.IsError && !result.IsCancelled)
                {
                    frame.texturePath = result.Path;

                    frame.ReloadTexture();
                }
            }
            ImGui.EndGroup();

            ImGui.EndGroup();
        }
    }
}
