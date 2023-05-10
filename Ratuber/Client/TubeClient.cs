using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET.MonoGame;
using System;
using Ratuber.Client.Data;
using Ratuber.Client.Data.Rules;
using ImGuiNET;

namespace Ratuber.Client
{
    public class TubeClient : Game
    {
        public ImGuiRenderer GuiRenderer { get; private set; }

        private const int SpoutFrameSize = 2048;
        private TuberRenderer tuberRenderer;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private bool firstTimeInit = true;
        private RenderTarget2D spoutTarget;
        private Spout.Interop.SpoutSender spoutSender;

        public TubeClient()
        {
            CurrentState.client = this;

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

            GuiRenderer = new ImGuiRenderer(this);

            tuberRenderer = new TuberRenderer(this);

            GuiRenderer.RebuildFontAtlas();

            ImGuiHelpers.Initialize(GraphicsDevice, GuiRenderer);

            Config.LoadConfig();

            Config.OnConfigUpdated += () =>
            {
                foreach (var layer in Config.CurrentConfig.LayerGroups)
                {
                    layer.Initialize(GraphicsDevice, GuiRenderer, firstTimeInit);
                }

                firstTimeInit = false;
            };

            Config.CurrentConfig.Initialize();

            spoutTarget = new RenderTarget2D(GraphicsDevice, SpoutFrameSize, SpoutFrameSize);

            spoutSender = new Spout.Interop.SpoutSender();
            spoutSender.CreateSender("Ratuber", SpoutFrameSize, SpoutFrameSize, 0);
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

            foreach (var layer in Config.CurrentConfig.LayerGroups)
            {
                layer.Update(gameTime);
            }

            RuleEvaluator.UpdateInputDictionary();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(spoutTarget);

            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            tuberRenderer.Draw(spriteBatch, TuberRenderer.GetSafeDrawRect(SpoutFrameSize, SpoutFrameSize));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            Spout.MonoGame.SpoutMono.SendTexture2D(spoutSender, spoutTarget);

            GraphicsDevice.Clear(Config.CurrentConfig.backgroundColor);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            tuberRenderer.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);

            GuiRenderer.BeforeLayout(gameTime);

            ImGuiConfigWindow.ConfigLayout();

            GuiRenderer.AfterLayout();
        }
    }
}