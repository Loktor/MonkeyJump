using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJumpGameModel
{
    public interface ICollidable
    {
        Collider Collider
        {
            get;
            set;
        }
    }
}
