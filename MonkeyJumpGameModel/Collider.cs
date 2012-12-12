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

        bool CenteredCollider { get; set; }

        public Rectangle CollisionBounds
        {
            get { return collisionBounds; }
            set { collisionBounds = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collisionBounds">Bounds of the collision rectangle</param>
        /// <param name="centeredCollider">centers the collider to the specified position</param>
        public Collider(Rectangle collisionBounds, bool centeredCollider)
        {
            CenteredCollider = centeredCollider;
            initialSize = new Size(collisionBounds.Width, collisionBounds.Height);
            CollisionBounds = collisionBounds;
        }

        public Collider(Vector2 position, Size size, bool centeredCollider)
        {
            CenteredCollider = centeredCollider;
            initialSize = size;
            int posX = centeredCollider ? (int)position.X - size.Width / 2 : (int)position.X;
            int posY = centeredCollider ? (int)position.Y - size.Height / 2 : (int)position.Y;
            CollisionBounds = new Rectangle(posX,posY,size.Width,size.Height);
        }

        public void MoveToPoint(Vector2 position)
        {
            collisionBounds.X = CenteredCollider ? (int)position.X - initialSize.Width / 2 : (int)position.X;
            collisionBounds.Y = CenteredCollider ? (int)position.Y - initialSize.Height / 2 : (int)position.Y;
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
