using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rattuber.Client
{
    public class AnimationTracker
    {
        public Vector2 position;
        public Texture2D Spritesheet { get; }
        public bool Loop { get; }
        public bool AnimationCompleted { get; private set; }
        public float timeBetweenFrames;

        private float FrameWidth => Spritesheet.Width / totalFrames;

        private float lastFrameRendered;
        private int frame;
        private readonly int totalFrames;

        public AnimationTracker(Texture2D spritesheet, int totalFrames, Vector2 startingPosition, bool loop = true, float timeBetweenFrames = 0.05f)
        {
            Spritesheet = spritesheet;
            this.totalFrames = totalFrames;
            Loop = loop;
            this.timeBetweenFrames = timeBetweenFrames;
            position = startingPosition;
        }

        public void Reset()
        {
            frame = 0;
            AnimationCompleted = false;
        }

        public void Update(GameTime gameTime)
        {
            if (AnimationCompleted) return;

            if (!(lastFrameRendered + timeBetweenFrames <= gameTime.TotalGameTime.TotalSeconds)) return;

            frame++;
            if (frame > totalFrames - 1)
            {
                if (Loop == false)
                {
                    AnimationCompleted = true;
                    return;
                }

                frame = 0;
            }

            lastFrameRendered = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Spritesheet,
                new Rectangle((int)position.X, (int)position.Y, (int)FrameWidth, Spritesheet.Height),
                new Rectangle((int)FrameWidth * frame, 0, (int)FrameWidth, Spritesheet.Height),
                Color.White);
        }
    }
}