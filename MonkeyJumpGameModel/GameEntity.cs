using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyJumpGameModel
{
    public abstract class GameEntity
    {
        internal Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public virtual void LoadTextures(ContentManager content) { }
        public virtual void Init(Rectangle gameBounds) { }
    }
}
