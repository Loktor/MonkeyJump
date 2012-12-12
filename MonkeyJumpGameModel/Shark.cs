using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class Shark : DrawableRotateEntity
    {
        private GameManager gameManager;
        private Rectangle gameBounds;
        private Direction headingDirection;
        private Size sharkSize = new Size(160, 80);
        private int sharkYPos = 750;

        public Shark()
            : base(new Size(160, 80))
        {
        }
       

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;
            this.gameBounds = gameBounds;
            position.X = 0-sharkSize.Width;
            position.Y = sharkYPos;
            texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.SHARK_PATH);
            headingDirection = Direction.Right;
        }

        public override void Update(GameTime gameTime)
        {
            position.X += (int)headingDirection * 2;
            Rotation = (float)Math.Sin((position.X * 5 / 100 - 45) ) / 3 - 0.2f * (int)headingDirection;
            position.Y = sharkYPos + (-(float)Math.Sin(position.X * 5 / 100) * 100) / 4 + 25 * (int)headingDirection;

            if (position.X < gameBounds.X-sharkSize.Width*1.5f || position.X > gameBounds.Right+sharkSize.Width/2)
            {
                headingDirection = (Direction)((int)headingDirection * -1);
                SpriteEffects = headingDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }
    }
}
