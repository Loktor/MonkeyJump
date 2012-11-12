using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class Coconut : DrawableEntity, ICollidable
    {
        private GameManager gameManager;
        private Size coconutSize = new Size(32, 38);
        public Collider Collider { get; set; }

        public override void Init(Rectangle gameBounds)
        {
            base.Init(gameBounds);
            gameManager = GameManager.Instance;
            position.X = new Random(DateTime.Now.Millisecond).Next(gameBounds.Width - 128) + gameBounds.X + 64;
            position.Y = -coconutSize.Height;
            Collider = new Collider(position, coconutSize,false);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds / 10 * GameManager.Instance.GameSpeed * 1.3f;
            Collider.MoveToPoint(position);
        }
    }
}
