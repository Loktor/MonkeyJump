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
        Size initialSize;

        public Rectangle CollisionBounds
        {
            get { return collisionBounds; }
            set { collisionBounds = value; }
        }

        public Collider(Rectangle collisionBounds)
        {
            initialSize = new Size(collisionBounds.Width, collisionBounds.Height);
            CollisionBounds = collisionBounds;
        }

        public Collider(Vector2 position,Size size)
        {
            initialSize = size;
            CollisionBounds = new Rectangle((int)position.X,(int)position.Y,size.Width,size.Height);
        }

        public void MoveToPoint(Vector2 position)
        {
            collisionBounds.X = (int)position.X;
            collisionBounds.Y = (int)position.Y;
        }

        public void Scale(float scaleFactor)
        {
            collisionBounds.Width = (int)(collisionBounds.Width * scaleFactor);
            collisionBounds.Height = (int)(collisionBounds.Height * scaleFactor);
        }

        public void ResetSizeModifications()
        {
            collisionBounds.Width = initialSize.Width;
            collisionBounds.Height = initialSize.Height;
        }

        public bool CollidesWith(Collider collider)
        {

            return collisionBounds.Intersects(collider.CollisionBounds);
        }

        public bool Contains(Collider collider)
        {
            return collisionBounds.Contains(collider.CollisionBounds);
        }

        public bool IsInside(Collider collider)
        {
            return collider.CollisionBounds.Contains(collisionBounds);
        }
    }
}
