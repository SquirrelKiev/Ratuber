using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Numerics;

namespace Ratuber.Client.Data
{
    public class Frame : UniqueObject, IDisposable
    {
        [JsonProperty]
        private string texturePath = string.Empty;

        /// <summary>
        /// How long the frame will last in milliseconds.
        /// </summary>
        public float frameLength;

        public bool isFrameLengthRandom;

        public Vector2 randomMinMax;

        private GraphicsDevice graphicsDevice;
        private ImGuiRenderer imGuiRenderer;

        [JsonIgnore]
        public Texture2D Texture { get; private set; }

        [JsonIgnore]
        public IntPtr ImGuiTexturePointer { get; private set; }

        public Frame Initialize(GraphicsDevice graphicsDevice, ImGuiRenderer imGuiRenderer, bool forceRefresh = true)
        {
            this.graphicsDevice = graphicsDevice;
            this.imGuiRenderer = imGuiRenderer;

            SetPath(texturePath, forceRefresh);

            return this;
        }

        public void SetPath(string path, bool forceRefresh = false)
        {
            if(texturePath == path && !forceRefresh)
                return; 
            
            texturePath = path;

            ReloadTexture();
        }

        // TODO: Cache textures/path for perf, adding layers/groups is too slow rn
        private void ReloadTexture()
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

        public void CalculateFrameLength()
        {
            if (!isFrameLengthRandom)
                return;

            var range = randomMinMax.Y - randomMinMax.X;

            frameLength = (CurrentState.random.NextSingle() * range) + randomMinMax.X;
        }

        ~Frame()
        {
            Dispose();
        }
    }
}
