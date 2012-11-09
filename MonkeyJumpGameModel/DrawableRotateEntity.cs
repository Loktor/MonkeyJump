using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonkeyJumpGameModel
{
    public abstract class DrawableRotateEntity : GameEntity
    {
        internal Texture2D texture;

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public Size FrameSize { get; set; }

        //A rectangle to store which 'frame' is currently being shown
        public Rectangle sourceRect;
        //The centre of the current 'frame'
        public Vector2 origin;

        public DrawableRotateEntity(Size size)
        {
            Rotation = 0.0f;
            Scale = 1.0f;
            SpriteEffects = SpriteEffects.None;
            FrameSize = size;
            sourceRect = new Rectangle(0, 0, FrameSize.Width, FrameSize.Height);
            origin = new Vector2(0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Draw(texture, Position, Color.White);
            spriteBatch.Draw(texture, position, sourceRect, Color.White, Rotation, origin, Scale, SpriteEffects, 0);
        
        }
    }
}
