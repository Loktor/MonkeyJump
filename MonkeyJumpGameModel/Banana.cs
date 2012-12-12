using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class Banana : DrawableRotateEntity, ICollidable, ICollectable
    {
        private GameManager gameManager;
        private Size bananaSize = new Size(32, 76);
        private const int SCORE_VALUE = 100;
        public Collider Collider { get; set; }
        public Direction Direction { get; set; }

        public Banana(Direction direction)
            : base(new Size(32, 76))
        {
            Direction = direction;
        }

        public Banana(Size s,Direction direction)
            : base(s)
        {
            bananaSize = s;
            Direction = direction;
        }

        public int Score
        {
            get
            {
                return SCORE_VALUE;
            }
        }

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;
            position.Y = -bananaSize.Height;
            Collider = BananaCollider(position, bananaSize, false,Direction);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed;
            Collider.MoveToPoint(position);
        }

        private MultiCollider BananaCollider(Vector2 position, Size size, bool centered, Direction direction)
        {
            MultiCollider climbCollider = new MultiCollider(position, size, centered, direction);
            climbCollider.AddCollider(new Collider(new Rectangle(1, 20, 15, 14), false));
            climbCollider.AddCollider(new Collider(new Rectangle(2, 1, 22, 17), false));
            climbCollider.AddCollider(new Collider(new Rectangle(2, 35, 19, 14), false));
            climbCollider.AddCollider(new Collider(new Rectangle(8, 50, 23, 15), false));
            return climbCollider;
        }
    }
}
