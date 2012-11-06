using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonkeyJumpGameModel
{
    public class Player : Character
    {
        Animation climbAnimation = null;
        Animation jumpAnimation = null;
        bool isJumping = false;
        Direction headingDirection = Direction.Left;

        public Player()
        {
            position.X = 60;
            position.Y = 600;
        }

        public override void Update(GameTime gameTime)
        {
            if (isJumping)
            {
                position.X += (int)headingDirection * GameManager.Instance.GameSpeed;
            }

            base.Update(gameTime);
        }

        public override void LoadTextures(ContentManager content)
        {
            Texture2D monkeyClimbSprite = content.Load<Texture2D>("game/monkeyClimbingSprite");
            climbAnimation = new Animation(monkeyClimbSprite, new Size(64, 64));
            Texture2D monkeyJumpSprite = content.Load<Texture2D>("game/monkeyJumpingSprite");
            jumpAnimation = new Animation(monkeyJumpSprite, new Size(64, 64));

            // Defines which animation should be played
            CurrentAnimation = climbAnimation;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        internal void HandleInput(TouchCollection touchCollection)
        {
            bool touchReleased = false;
            foreach (TouchLocation touch in touchCollection)
            {
                if (touch.State == TouchLocationState.Released)
                {
                    touchReleased = true;
                }
            }
            if (touchReleased && !isJumping)
            {
                isJumping = true;
                if(CurrentAnimation != jumpAnimation)
                    CurrentAnimation = jumpAnimation;
                headingDirection = (Direction)((int)headingDirection * -1);
                jumpAnimation.SpriteEffects = headingDirection == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }
    }
}
