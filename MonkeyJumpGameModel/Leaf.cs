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

        public Direction Direction { get; set; }

        public Leaf()
            : base(new Size(154, 64))
        {
        }

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;

            Direction = new Random().Next(2) == 0 ? Direction.Left : Direction.Right;
            SpriteEffects = Direction == Direction.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            position.X = Direction == Direction.Left ? gameBounds.X / 2 : gameBounds.Right + gameBounds.X / 2 - leafSize.Width;
            position.Y = -(leafSize.Height +70);
            Collider = LeafCollider(position, leafSize, false, Direction);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed;
            Collider.MoveToPoint(position);
        }

        private MultiCollider LeafCollider(Vector2 position, Size size, bool centered,Direction direction)
        {
            MultiCollider climbCollider = new MultiCollider(position, size, centered, direction);
            climbCollider.AddCollider(new Collider(new Rectangle(0, leafSize.Height / 2 - 10, leafSize.Width - 20, 20), false));
            climbCollider.AddCollider(new Collider(new Rectangle((int)(leafSize.Width * 0.2f), (int)(leafSize.Height * 0.1f), (int)(leafSize.Width * 0.5f), leafSize.Height / 4), false));
            return climbCollider;
        }
    }
}
