using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class LoopingBackground : DrawableEntity
    {
        public LoopingBackground()
        {
            position.X = 0;
            position.Y = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.Y >= 0)
            {
                int overflow = (int)Position.Y;
                ResetPosition(overflow);
            }
            else
            {
                Console.WriteLine("Speed: " + gameTime.ElapsedGameTime.Milliseconds);
                position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed;//GameManager.Instance.GameSpeed;
            }
        }

        public override void LoadTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("game/palmBGLoop");
        }


        public void ResetPosition(int overflow)
        {
            Rectangle titleSaveArea = GameManager.Instance.Screen.TitleSafeArea;
            position.X = titleSaveArea.X;
            position.Y = titleSaveArea.Y - titleSaveArea.Height + overflow;
        }
    }
}