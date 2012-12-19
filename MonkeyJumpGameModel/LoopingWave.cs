using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace MonkeyJumpGameModel
{
    public class LoopingWave : DrawableEntity
    {
        Texture2D bloodTexture;

        float bloodPercentage;
        bool showBloodEffekt;
        public bool BloodEffektFinished { get; set; }

        bool isRight;
        bool isLow;
        public LoopingWave(bool isRight, bool isLow)
        {
            BloodEffektFinished = false;
            position.X = isRight ? 0 : 0;
            position.Y = isRight ? 710 : 690;
            position.Y = isLow ? 750 : position.Y;
            this.isRight = isRight;
            this.isLow = isLow;
        }

        public override void LoadTextures(ContentManager content)
        {
            String texName = isRight ? "game/highWavesRight" : isLow ? "game/lowWaves" : "game/highWavesLeft";
            texture = content.Load<Texture2D>(texName);
            texName = isRight ? "game/highWavesRightBlood" : isLow ? "game/lowWavesBlood" : "game/highWavesLeftBlood";
            bloodTexture = GameManager.Instance.ResourceManager.RetreiveTexture(texName);
        }

        public void StartBloodEffekt()
        {
            bloodPercentage = 0;
            showBloodEffekt = true;
        }


        public override void Update(GameTime gameTime)
        {
            if (showBloodEffekt && bloodPercentage < 1)
            {
                bloodPercentage += 0.05f;
            }
            else if (bloodPercentage > 1 && BloodEffektFinished == false)
            {
                texture = bloodTexture;
                BloodEffektFinished = true;
            }
            if (Position.X >= 0 && isRight || Position.X <= -480 && !isRight)
            {
                int overflow = (int)Position.X;
                ResetPosition(overflow);
            }
            else
            {
                Console.WriteLine("Speed: " + gameTime.ElapsedGameTime.Milliseconds);
                float waveSpeed = isRight ? 2.5f : isLow ? 2f : 3f;
                position.X += 3 / waveSpeed * ((isRight) ? 1 : -1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            if (showBloodEffekt)
            {
                
                spriteBatch.Draw(bloodTexture, Position, Color.White * bloodPercentage);
            }
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
