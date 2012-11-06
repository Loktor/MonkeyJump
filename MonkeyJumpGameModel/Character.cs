using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace MonkeyJumpGameModel
{
    public abstract class Character : AnimationEntity, ICollidable
    {
        public Collider Collider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
