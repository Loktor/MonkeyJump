using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyJumpGameModel
{
    public class Player : AnimationEntity, ICollidable
    {
        private Animation climbAnimation = null;
        private Animation jumpAnimation = null;
        private PlayerState playerState;
        private Direction headingDirection;
        private GameManager gameManager;
        private Rectangle gameBounds;
        private int gameYCenter;
        private Collider collider;
        private Size monkeySize = new Size(56, 56);
        private int monkeyYLevel = 600;
        private int playerScore = 0;
        private int bananaScore = 0;
        private bool isImmortal = false;
        private int immortalTime = 0;
        // Playable values between 15 and 80. Higher -> more gravity, lower jump
        int monkeyJumpGravity = 25;

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }

        public int BananaScore
        {
            get { return bananaScore; }
            set { bananaScore = value; }
        }

        public bool IsImmortal
        {
            get { return isImmortal; }
            set { isImmortal = value; }
        }

        public PlayerState PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

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
            playerState = PlayerState.Climbing;
            collider = new Collider(position, monkeySize,true);
            this.gameBounds = gameBounds;
            gameYCenter = (gameBounds.Width / 2) + gameBounds.X;
        }

        public override void Update(GameTime gameTime)
        {
            if (playerState != PlayerState.Dying && playerState != PlayerState.Dead)
            {
                playerScore += 1;
                if (isImmortal)
                {
                    immortalTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (immortalTime > 2000)
                        isImmortal = false;
                }
            }
            if (playerState == PlayerState.Jumping)
            {
                position.X += (int)headingDirection * (10 + GameManager.Instance.GameSpeed);
                position.Y = position.X > gameYCenter ? position.Y + (1 * ((position.X - gameYCenter) / monkeyJumpGravity) * (int)headingDirection) : position.Y - (1 * ((gameYCenter - position.X) / monkeyJumpGravity) * (int)headingDirection);
                collider.MoveToPoint(position);

                if (position.X < gameBounds.X || position.X > gameBounds.Right)
                {
                    position.X = position.X < gameBounds.X ? gameBounds.X : gameBounds.Right;
                    position.Y = monkeyYLevel;
                    collider.MoveToPoint(position);
                    playerState = PlayerState.Climbing;
                    CurrentAnimation = climbAnimation;
                }
            }
            else if (playerState == PlayerState.Dying)
            {
                position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed * 1.5f;
                collider.MoveToPoint(position);

                if (position.Y > gameBounds.Bottom)
                {
                    playerState = PlayerState.Dead;
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
            if (touchReleased && playerState == PlayerState.Climbing)
            {
                playerState = PlayerState.Jumping;
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

        public void KillPlayer()
        {
            if (playerState != PlayerState.Dying)
            {
                playerState = PlayerState.Dying;
                climbAnimation.Rotation = headingDirection == Direction.Left ? 1.57f : 4.71f;
                CurrentAnimation = climbAnimation;

                SoundEffect dieSound = gameManager.ResourceManager.RetreiveSong(ResourceManager.MONKEY_DEATH_SOUND);
                dieSound.Play();
            }
        }

        public void startImmortality()
        {
            isImmortal = true;
            immortalTime = 0;
        }
    }
}
