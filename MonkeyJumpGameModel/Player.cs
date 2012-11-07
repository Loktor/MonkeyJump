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
    public class Player : AnimationEntity, ICollidable
    {
        Animation climbAnimation = null;
        Animation jumpAnimation = null;
        bool isJumping = false;
        Direction headingDirection;
        GameManager gameManager;
        Rectangle gameBounds;
        int gameYCenter;
        Collider collider;
        Size monkeySize = new Size(64, 64);
        int monkeyYLevel = 600;

        // Playable values between 15 and 80. Higher -> more gravity, lower jump
        int monkeyJumpGravity = 25; 

        public Collider Collider
        {
            get
            {
                return collider;
            }
            set
            {
                Collider = value;
            }
        }

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;
            position.X = gameBounds.X;
            position.Y = monkeyYLevel;
            headingDirection = Direction.Left;
            collider = new Collider(position, monkeySize,true);
            this.gameBounds = gameBounds;
            gameYCenter = (gameBounds.Width / 2) + gameBounds.X;
        }

        public override void Update(GameTime gameTime)
        {
            if (isJumping)
            {

                position.X += (int)headingDirection * gameManager.GameSpeed * 2;
                position.Y = position.X > gameYCenter ? position.Y + (1 * ((position.X - gameYCenter) / monkeyJumpGravity) * (int)headingDirection) : position.Y - (1 * ((gameYCenter - position.X) / monkeyJumpGravity) * (int)headingDirection);
                collider.MoveToPoint(position);

                if (position.X < gameBounds.X || position.X > gameBounds.Right)
                {
                    position.X = position.X < gameBounds.X ? gameBounds.X : gameBounds.Right;
                    position.Y = monkeyYLevel;
                    collider.MoveToPoint(position);
                    isJumping = false;
                    CurrentAnimation = climbAnimation;
                }
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
                FitAnimationDirection();
            }
        }

        private void FitAnimationDirection()
        {
            jumpAnimation.SpriteEffects = headingDirection == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            climbAnimation.SpriteEffects = headingDirection == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }
    }
}
