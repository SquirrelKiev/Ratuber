using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET.MonoGame;
using System;
using Ratuber.Client.Data;
using Ratuber.Client.Data.Rules;

namespace Ratuber.Client
{
    public class TubeClient : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private ImGuiRenderer guiRenderer;
        private TuberRenderer tuberRenderer;

        public TubeClient()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = true;

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            graphics.ApplyChanges();

            Exiting += TubeClient_Exiting;

            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            guiRenderer = new ImGuiRenderer(this);

            tuberRenderer = new TuberRenderer(this);

            guiRenderer.RebuildFontAtlas();

            Config.LoadConfig();

            Config.OnConfigUpdated += () =>
            {
                foreach (var layer in Config.CurrentConfig.Layers)
                {
                    layer.Initialize(GraphicsDevice, guiRenderer);
                }
            };

            Config.CurrentConfig.Initialize();
        }

        private void TubeClient_Exiting(object sender, EventArgs e)
        {
            Config.SaveConfig();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var micMan in CurrentState.MicrophoneManagers)
            {
                micMan.Update(gameTime);
            }

            RuleEvaluator.UpdateInputDictionary();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            tuberRenderer.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);

            guiRenderer.BeforeLayout(gameTime);

            ImGuiConfigWindow.ConfigLayout();

            guiRenderer.AfterLayout();
        }
    }
}