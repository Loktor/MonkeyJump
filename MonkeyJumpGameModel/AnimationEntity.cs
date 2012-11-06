using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonkeyJumpGameModel
{
    public abstract class AnimationEntity : GameEntity
    {
        public abstract List<MonkeyJumpGameModel.Animation> Animations { get; set; }

        public abstract Animation CurrentAnimation { get; set; }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(CurrentAnimation.Texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
