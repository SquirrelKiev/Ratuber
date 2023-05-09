using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Ratuber.Client.Data
{
    public class Frame : UniqueObject, IDisposable
    {
        public string texturePath = string.Empty;
        public float frameLength;
        private GraphicsDevice graphicsDevice;
        private ImGuiRenderer imGuiRenderer;

        [JsonIgnore]
        public Texture2D Texture { get; private set; }

        [JsonIgnore]
        public IntPtr ImGuiTexturePointer { get; private set; }

        public Frame Initialize(GraphicsDevice graphicsDevice, ImGuiRenderer imGuiRenderer)
        {
            this.graphicsDevice = graphicsDevice;
            this.imGuiRenderer = imGuiRenderer;

            return this;
        }

        public void ReloadTexture()
        {
            FreeTextures();

            if (File.Exists(texturePath))
            {
                Texture = Texture2D.FromFile(graphicsDevice, texturePath);

                ImGuiTexturePointer = imGuiRenderer.BindTexture(Texture);
            }
        }

        public void Dispose()
        {
            FreeTextures();

            GC.SuppressFinalize(this);
        }

        private void FreeTextures()
        {
            Texture?.Dispose();

            if (ImGuiTexturePointer != IntPtr.Zero)
                imGuiRenderer.UnbindTexture(ImGuiTexturePointer);
        }

        ~Frame()
        {
            Dispose();
        }
    }
}
