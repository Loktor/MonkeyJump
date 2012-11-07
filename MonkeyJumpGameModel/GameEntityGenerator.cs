using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public abstract class GameEntityGenerator
    {
        public abstract List<GameEntity> GenerateEntities(GameTime gameTime);
    }
}
