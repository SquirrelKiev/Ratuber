using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ratuber.Client.Data;

namespace Ratuber.Client
{
    public class TuberRenderer
    {
        private Game game;

        private Rectangle drawRect;

        public TuberRenderer(Game game)
        {
            this.game = game;

            game.Window.ClientSizeChanged += (_, _) => OnWindowResize();

            OnWindowResize();
        }

        public static Rectangle GetSafeDrawRect(int width, int height)
        {
            var size = new Point(height, height);

            var position = new Point(width / 2 - size.X / 2, height - size.Y);

            return new Rectangle(position, size);
        }

        public void OnWindowResize()
        {
            drawRect = GetSafeDrawRect(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, drawRect);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawRect)
        {
            foreach (var layerGroup in Config.CurrentConfig.LayerGroups)
            {
                if (!layerGroup.ShouldRender())
                    continue;

                foreach (var layer in layerGroup.layers)
                {
                    if (!layer.ShouldRender())
                        continue;

                    var tex = layer.GetNextTexture();

                    if (tex != null)
                        spriteBatch.Draw(tex, drawRect, tex.Bounds, Color.White);
                }
            }
        }
    }
}
