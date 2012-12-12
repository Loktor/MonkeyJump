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
        public AnimationEntity()
        {
            CurrentAnimation = null;
        }

        public Animation CurrentAnimation { get; set; }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CheckIfCurrentAnimationValid();
            CurrentAnimation.Draw(spriteBatch, position);
        }

        public override void Update(GameTime gameTime)
        {
            CheckIfCurrentAnimationValid();
            CurrentAnimation.Update(gameTime);
        }

        /// <summary>
        /// Displays an error message if the current animation wasn't set yet
        /// </summary>
        private void CheckIfCurrentAnimationValid()
        {
            if (CurrentAnimation == null)
            {
                throw new NullReferenceException("No CurrentAnimation set -> Null Pointer");
            }
        }
    }
}
