using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonkeyJumpGameModel
{
    public class Player : Character
    {

        public Player()
        {
            position.X = 60;
            position.Y = 600;
        }
        //Sprite Texture
        Texture2D sprite;
        //A Timer variable
        float timer = 0f;
        //The interval (100 milliseconds)
        float interval = 100f;
        //Current frame holder (start at 1)
        int currentFrame = 1;
        //Width of a single sprite image, not the whole Sprite Sheet
        int spriteWidth = 64;
        //Height of a single sprite image, not the whole Sprite Sheet
        int spriteHeight = 64;
        //A rectangle to store which 'frame' is currently being shown
        Rectangle sourceRect;
        //The centre of the current 'frame'
        Vector2 origin;

        bool isJumping = false;

        public override void Update(GameTime gameTime)
        {

            

            if (isJumping)
            {
                currentFrame = 3;
            }
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //Check the timer is more than the chosen interval
                if (timer > interval)
                {
                    //Show the next frame
                    currentFrame++;
                    //Reset the timer
                    timer = 0f;
                }
                //If we are on the last frame, reset back to the one before the first frame (because currentframe++ is called next so the next frame will be 1!)
                if (currentFrame == 3)
                {
                    currentFrame = 0;
                }
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

            base.Update(gameTime);
            
        }

        public override void LoadTextures(ContentManager content)
        {
            sprite = content.Load<Texture2D>("game/monkeySprite");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //Draw the sprite in the centre of an 800x600 screen
            spriteBatch.Draw(sprite, new Vector2(Position.X, Position.Y), sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0);

            base.Draw(spriteBatch, gameTime);
        }

    }
}
