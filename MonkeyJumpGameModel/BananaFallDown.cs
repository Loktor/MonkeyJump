using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class BananaFallDown : DrawableEntity
    {
        private GameManager gameManager;
        private Size bananaSize = new Size(16, 34);

        private List<Banana> bananas;

        private Random r = new Random();

        public void Init(Rectangle gameBounds, Vector2 pos)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;

            bananas = new List<Banana>();

            for(int i = 0; i<3; i++)
            {
                Banana b = new Banana(bananaSize,Direction.Left);

                b.texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH);
                b.Init(gameManager.GameBounds);

                b.Origin = new Vector2(bananaSize.Width/2, bananaSize.Height/2);

                b.position = pos;
                b.position.X += r.Next(30)-15;
                b.position.Y += r.Next(30)-15;

                b.Rotation = (float)r.NextDouble() * 3;

                b.SpriteEffects = r.Next(2) == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                bananas.Add(b);
            }


        }

        public override void Update(GameTime gameTime)
        {
            foreach (Banana b in bananas)
            {
                b.position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed * 1.5f;
                b.Rotation += b.SpriteEffects == SpriteEffects.None ? 0.2f : -0.3f;
            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Banana b in bananas)
            {
                b.Draw(spriteBatch, gameTime);
            }
        }
    }
}

