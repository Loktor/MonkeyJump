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
        internal float currentScale = 1;

        public bool CenteredCollider { get; set; }

        public Rectangle CollisionBounds
        {
            get { return collisionBounds; }
            set { collisionBounds = value; }
        }

        public Rectangle InitBounds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collisionBounds">Bounds of the collision rectangle</param>
        /// <param name="centeredCollider">Defines if the position is in the middle or the top left (centered = middle)</param>
        public Collider(Rectangle collisionBounds, bool centeredCollider)
        {
            CenteredCollider = centeredCollider;
            CollisionBounds = collisionBounds;
            InitBounds = collisionBounds;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Position of the collider on the screen</param>
        /// <param name="size">Initial size of the collider</param>
        /// <param name="centeredCollider">Defines if the position is in the middle or the top left (centered = middle)</param>
        public Collider(Vector2 position, Size size, bool centeredCollider)
        {
            CenteredCollider = centeredCollider;
            int posX = centeredCollider ? (int)position.X - size.Width / 2 : (int)position.X;
            int posY = centeredCollider ? (int)position.Y - size.Height / 2 : (int)position.Y;
            CollisionBounds = new Rectangle(posX,posY,size.Width,size.Height);
            InitBounds = CollisionBounds;
        }

        public virtual void MoveToPoint(Vector2 position)
        {
            collisionBounds.X = CenteredCollider ? (int)position.X - InitBounds.Width / 2 : (int)position.X;
            collisionBounds.Y = CenteredCollider ? (int)position.Y - InitBounds.Height / 2 : (int)position.Y;
        }

        public virtual void Scale(float scaleFactor)
        {
            currentScale = scaleFactor;
            collisionBounds.Width = (int)(InitBounds.Width * scaleFactor);
            collisionBounds.Height = (int)(InitBounds.Height * scaleFactor);
        }

        public virtual void ResetSizeModifications()
        {
            collisionBounds.Width = InitBounds.Width;
            collisionBounds.Height = InitBounds.Height;
        }

        public virtual bool CollidesWith(Collider collider)
        {
            if (collider is MultiCollider && collisionBounds.Intersects(collider.CollisionBounds))
            {
                foreach (Collider col in ((MultiCollider)collider).Colliders)
                {
                    if (collisionBounds.Intersects(col.CollisionBounds))
                        return true;
                }
                return false;
            }
            return collisionBounds.Intersects(collider.CollisionBounds);
        }

        public virtual bool Contains(Collider collider)
        {
            return collisionBounds.Contains(collider.CollisionBounds);
        }

        public virtual bool IsInside(Collider collider)
        {
            return collider.CollisionBounds.Contains(collisionBounds);
        }
    }
}
