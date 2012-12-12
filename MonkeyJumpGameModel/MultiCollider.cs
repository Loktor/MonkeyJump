using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MonkeyJumpGameModel
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MultiCollider : Collider
    {
        List<Collider> colliders = new List<Collider>();

        public List<Collider> Colliders
        {
            get { return colliders; }
            set { colliders = value; }
        }

        public MultiCollider(Rectangle collisionBounds, bool centeredCollider)
            : base(collisionBounds, centeredCollider)
        {

        }

        public MultiCollider(Vector2 position, Size size, bool centeredCollider)
            : base(position, size, centeredCollider)
        {

        }

        public MultiCollider(Rectangle collisionBounds, bool centeredCollider, List<Collider> colliders)
            : base(collisionBounds, centeredCollider)
        {
            Colliders = colliders;
        }

        public MultiCollider(Vector2 position, Size size, bool centeredCollider, List<Collider> colliders)
            : base(position, size, centeredCollider)
        {
            Colliders = colliders;
        }

        public void AddCollider(Collider col)
        {
            col.MoveToPoint(CalculateCurrentColPos(col));
            Colliders.Add(col);
        }

        public override void MoveToPoint(Vector2 position)
        {
            base.MoveToPoint(position);
            foreach (Collider col in Colliders)
            {
                col.MoveToPoint(CalculateCurrentColPos(col));
            }
        }

        public override void Scale(float scaleFactor)
        {
            base.Scale(scaleFactor);
            foreach (Collider col in Colliders)
            {
                col.Scale(scaleFactor);
            }
        }

        public override void ResetSizeModifications()
        {
            base.ResetSizeModifications();
            foreach (Collider col in Colliders)
            {
                col.ResetSizeModifications();
            }
        }

        public override bool CollidesWith(Collider collider)
        {
            if (!base.CollidesWith(collider))
            {
                return false;
            }
            foreach (Collider col in Colliders)
            {
                if (col.CollidesWith(collider)) return true;
            }
            return false;
        }

        public override bool Contains(Collider collider)
        {
            if (base.Contains(collider))
            {
                return true;
            }
            foreach (Collider col in Colliders)
            {
                if (col.Contains(collider)) return true;
            }
            return false;
        }

        public override bool IsInside(Collider collider)
        {
            if (base.IsInside(collider))
            {
                return true;
            }
            foreach (Collider col in Colliders)
            {
                if (col.Contains(collider)) return true;
            }
            return false;
        }

        private Vector2 CalculateCurrentColPos(Collider col)
        {
            return new Vector2(base.CollisionBounds.X + (col.InitBounds.X * currentScale), base.CollisionBounds.Y + (col.InitBounds.Y * currentScale));
        }
    }
}
