using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class Leaf : DrawableRotateEntity, ICollidable
    {
        private GameManager gameManager;
        private Size leafSize = new Size(154, 64);
        public Collider Collider { get; set; }

        private bool isLeft;

        public Leaf()
            : base(new Size(154, 64))
        {
        }

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;

            isLeft = new Random().Next(2) == 0;
            SpriteEffects = isLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            position.X = isLeft ? gameBounds.X / 2 : gameBounds.Right + gameBounds.X / 2 - leafSize.Width;
            position.Y = -leafSize.Height;
            Collider = new Collider(position, leafSize,false);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameManager.GameSpeed;
            Collider.MoveToPoint(position);
        }
    }
}
