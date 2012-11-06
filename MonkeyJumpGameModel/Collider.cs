using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class Collider
    {
        Rectangle collisionBounds;

        public bool CheckCollision(Collider collider)
        {
            return collisionBounds.Intersects(collider.collisionBounds);
        }
    }
}
