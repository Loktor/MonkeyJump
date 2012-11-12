using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace MonkeyJumpGameModel
{
    public class LoopingWaves : DrawableEntity
    {
        bool isRight;
        bool isLow;
        public LoopingWaves(bool isRight, bool isLow)
        {
            position.X = isRight ? 0 : 0;
            position.Y = isRight ? 710 : 690;
            position.Y = isLow ? 750 : position.Y;
            this.isRight = isRight;
            this.isLow = isLow;
        }

        public override void Update(GameTime gameTime)
        {

            if (Position.X >= 0 && isRight || Position.X <= -480 && !isRight)
            {
                int overflow = (int)Position.X;
                ResetPosition(overflow);
            }
            else
            {
                Console.WriteLine("Speed: " + gameTime.ElapsedGameTime.Milliseconds);
                float waveSpeed = isRight ? 2.5f : isLow ? 2f : 3f;
                position.X += 3/waveSpeed * ((isRight) ? 1 : -1);
            }
        }

        public override void LoadTextures(ContentManager content)
        {
            String texName = isRight ? "game/highWavesRight" : isLow ? "game/lowWaves" : "game/highWavesLeft";
            texture = content.Load<Texture2D>(texName);
        }


        public void ResetPosition(int overflow)
        {
            Rectangle titleSaveArea = GameManager.Instance.Screen.TitleSafeArea;

            if (isRight)
            {
                position.X = titleSaveArea.X - titleSaveArea.Width + overflow;
            }
            else
            {
                position.X = 0 - overflow-480;
            }
        }
    }
}
