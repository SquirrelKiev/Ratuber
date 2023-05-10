using ImGuiNET;
using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Ratuber.Client.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ratuber.Client
{
    internal static class ImGuiHelpers
    {
        private static GraphicsDevice graphicsDevice;
        private static ImGuiRenderer imGuiRenderer;

        private static Texture2D fallbackTexture;
        private static IntPtr fallbackTexturePtr;
        private static string fallbackTexturePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "fallback.png");

        public static void Initialize(GraphicsDevice _graphicsDevice, ImGuiRenderer _imGuiRenderer)
        {
            graphicsDevice = _graphicsDevice;

            imGuiRenderer = _imGuiRenderer;
        }

        public static void MoveElementArrows<T>(IList<T> list, T item, bool horizontal = false) where T : UniqueObject
        {
            ImGui.BeginGroup();

            if (ImGui.ArrowButton($"##frameUp-{item.GetUniqueId()}", ImGuiDir.Up))
            {
                list.Move(item, list.IndexOf(item) - 1);
            }

            if (horizontal)
                ImGui.SameLine();

            if (ImGui.ArrowButton($"##frameDown-{item.GetUniqueId()}", ImGuiDir.Down))
            {
                list.Move(item, list.IndexOf(item) + 1);
            }

            ImGui.EndGroup();
        }

        public static void HelpTooltip(string desc)
        {
            ImGui.TextDisabled("(?)");

            if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayShort))
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
                ImGui.Text(desc);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
        }

        public static IntPtr GetSafeTexture(IntPtr texture)
        {
            if (texture == IntPtr.Zero)
            {
                if(fallbackTexturePtr == IntPtr.Zero)
                {
                    LoadFallbackTexture();
                }

                return fallbackTexturePtr;
            }
            else
            {
                return texture;
            }
        }

        private static void LoadFallbackTexture()
        {
            fallbackTexture = Texture2D.FromFile(graphicsDevice, fallbackTexturePath);

            fallbackTexturePtr = imGuiRenderer.BindTexture(fallbackTexture);
        }
    }
}
