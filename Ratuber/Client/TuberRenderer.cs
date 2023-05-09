using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ratuber.Client.Data;

namespace Ratuber.Client
{
    public class TuberRenderer
    {
        private Game game;

        private Point size;
        private Point position;
        private Rectangle drawRect;

        public TuberRenderer(Game game)
        {
            this.game = game;

            game.Window.ClientSizeChanged += (_, _) => OnWindowResize();

            OnWindowResize();
        }

        public void OnWindowResize()
        {
            size = new Point(game.Window.ClientBounds.Height, game.Window.ClientBounds.Height);

            position = new Point(game.Window.ClientBounds.Width / 2 - size.X / 2, game.Window.ClientBounds.Height - size.Y);

            drawRect = new Rectangle(position, size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in Config.CurrentConfig.Layers)
            {
                if (layer.ShouldRender())
                {
                    var tex = layer.GetNextTexture();

                    if (tex != null)
                        spriteBatch.Draw(tex, drawRect, tex.Bounds, Color.White);
                }
            }
        }
    }
}
