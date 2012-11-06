using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyJumpGameModel
{
    public class Animation
    {
        /// <summary>
        /// Sprite Texture
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Frame width and height used to draw 
        /// </summary>
        public Size FrameSize { get; set; }

        /// <summary>
        /// Current frame holder (start at 0)
        /// </summary>
        public int CurrentFrame { get; set; }

        /// <summary>
        /// The interval in which the animation gets updated
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// Amount of frames in the sprite
        /// </summary>
        public int FrameCount
        {
            get
            {
                return Texture.Width / (int)FrameSize.Width;
            }
        }

        public float Scale { get; set; }

        public Color TintColor { get; set; }

        public float Rotation { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public int LayerDepth { get; set; }

        //A Timer variable
        private float timer = 0f;
        //A rectangle to store which 'frame' is currently being shown
        private Rectangle sourceRect;
        //The centre of the current 'frame'
        private Vector2 origin;

        public Animation(Texture2D texture, Size frameSize)
            : this(texture, frameSize, 100f, 1.0f, 0.0f)
        {
        }

        public Animation(Texture2D texture, Size frameSize, float frameUpdateInterval, float scale, float rotation) 
            : this(texture, frameSize, frameUpdateInterval, scale, Color.White, rotation, SpriteEffects.None, 0)
        {
        }

        public Animation(Texture2D texture, Size frameSize, float frameUpdateInterval, float scale,
            Color tintColor, float rotation, SpriteEffects spriteEffects, int layerDepth)
        {
            timer = 0f;
            CurrentFrame = 0;
            Texture = texture;
            FrameSize = frameSize;
            Interval = frameUpdateInterval;
            Scale = scale;
            TintColor = tintColor;
            Rotation = rotation;
            SpriteEffects = spriteEffects;
            LayerDepth = layerDepth;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Check the timer is more than the chosen interval
            if (timer > Interval)
            {
                //Show the next frame
                CurrentFrame = (CurrentFrame+1 >= FrameCount) ? 0 : CurrentFrame+1;
                //Reset the timer
                timer = 0f;
            }
            sourceRect = new Rectangle(CurrentFrame * FrameSize.Width, 0, FrameSize.Width, FrameSize.Height);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
             //Draw the sprite in the centre of an 800x600 screen
            spriteBatch.Draw(Texture, position, sourceRect, TintColor, Rotation, origin, Scale, SpriteEffects, LayerDepth);
        }
    }
}
