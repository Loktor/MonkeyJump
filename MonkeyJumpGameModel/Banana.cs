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

        public Banana()
            : base(new Size(32, 76))
        {
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
            //position.X = new Random(DateTime.Now.Millisecond).Next(gameBounds.Width - bananaSize.Width) + gameBounds.X;
            position.Y = -bananaSize.Height;
            Collider = new Collider(position, bananaSize, false);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed;
            Collider.MoveToPoint(position);
        }
    }
}
