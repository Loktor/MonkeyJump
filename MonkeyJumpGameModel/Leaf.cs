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

        public bool isLeft;

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
            position.Y = -(leafSize.Height +70);
            Collider = LeafCollider(position, leafSize, false);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed;
            Collider.MoveToPoint(position);
        }

        private MultiCollider LeafCollider(Vector2 position, Size size, bool centered)
        {
            MultiCollider climbCollider = new MultiCollider(position, size, centered);
            climbCollider.AddCollider(new Collider(new Rectangle(0, leafSize.Height / 2 - 10, leafSize.Width - 20, 20), false));
            climbCollider.AddCollider(new Collider(new Rectangle((int)(leafSize.Width * 0.2f), (int)(leafSize.Height * 0.1f), (int)(leafSize.Width * 0.5f), leafSize.Height / 4), false));
            return climbCollider;
        }
    }
}
