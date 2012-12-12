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
        Direction direction = Direction.Left;

        public List<Collider> Colliders
        {
            get { return colliders; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collisionBounds">Bounds of the collision rectangle</param>
        /// <param name="centeredCollider">centers the collider to the specified position</param>
        /// <param name="direction">Direction in which the collider should be facing (used for finding the correct possition of subcolliders</param>
        public MultiCollider(Rectangle collisionBounds, bool centeredCollider,Direction direction)
            : base(collisionBounds, centeredCollider)
        {
            this.direction = direction;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Position of the multi collider on the screen</param>
        /// <param name="size">Initial size of the multi collider</param>
        /// <param name="centeredCollider">defines if the position is in the middle or the top left (centered = middle)</param>
        /// <param name="direction">Direction in which the collider should be facing (used for finding the correct possition of subcolliders</param>
        public MultiCollider(Vector2 position, Size size, bool centeredCollider, Direction direction)
            : base(position, size, centeredCollider)
        {
            this.direction = direction;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collisionBounds">Bounds of the collision rectangle</param>
        /// <param name="centeredCollider">defines if the position is in the middle or the top left (centered = middle)</param>
        /// <param name="colliders">List off colliders each collider will be adjusted to the base position (coordinates of subcolliders need to start at 0/0)</param>
        /// <param name="direction">Direction in which the collider should be facing (used for finding the correct possition of subcolliders</param>
        public MultiCollider(Rectangle collisionBounds, bool centeredCollider, List<Collider> colliders, Direction direction)
            : base(collisionBounds, centeredCollider)
        {
            foreach (Collider col in colliders)
            {
                col.MoveToPoint(CalculateCurrentColPos(col));
            }
            this.colliders = colliders;
            this.direction = direction;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Position of the multi collider on the screen</param>
        /// <param name="size">Initial size of the multi collider</param>
        /// <param name="centeredCollider">defines if the position is in the middle or the top left (centered = middle)</param>
        /// <param name="colliders">List off colliders each collider will be adjusted to the base position (coordinates of subcolliders need to start at 0/0)</param>
        /// <param name="direction">Direction in which the collider should be facing (used for finding the correct possition of subcolliders</param>
        public MultiCollider(Vector2 position, Size size, bool centeredCollider, List<Collider> colliders, Direction direction)
            : base(position, size, centeredCollider)
        {
            foreach (Collider col in colliders)
            {
                col.MoveToPoint(CalculateCurrentColPos(col));
            }
            this.colliders = colliders;
            this.direction = direction;
        }

        /// <summary>
        /// Add an new collider to the multi collider important the new collider has to be placed relativ to 0,0 the multi collider will do the positionning
        /// himself
        /// </summary>
        /// <param name="col">Collider to add</param>
        public void AddCollider(Collider col)
        {
            col.MoveToPoint(CalculateCurrentColPos(col));
            Colliders.Add(col);
        }

        /// <summary>
        /// Change the heading direction of the collider
        /// </summary>
        /// <param name="dir"></param>
        public void ChangeDirection(Direction dir)
        {
            direction = dir;
            MoveToPoint(new Vector2(base.CollisionBounds.X,base.CollisionBounds.Y));
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
            if (direction == Direction.Left)
            {
                return new Vector2(base.CollisionBounds.X + (col.InitBounds.X * currentScale), base.CollisionBounds.Y + (col.InitBounds.Y * currentScale));
            }
            else if(direction == Direction.Right)
            {
                return new Vector2((base.CollisionBounds.X + base.CollisionBounds.Width) - (col.InitBounds.X * currentScale + col.CollisionBounds.Width), base.CollisionBounds.Y + (col.InitBounds.Y * currentScale));
            }
            return new Vector2();
        }
    }
}
