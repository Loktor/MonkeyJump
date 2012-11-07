using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class CoconutGenerator : GameEntityGenerator
    {
        int elapsedTime = 0;
        int generationTime = 1000;

        public override List<GameEntity> GenerateEntities(GameTime gameTime)
        {
            List<GameEntity> generatedEntities = new List<GameEntity>();
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            while (elapsedTime > generationTime)
            {
                elapsedTime -= generationTime;
                generatedEntities.Add(CreateRandomCoconut());
            }
            if (generatedEntities.Count > 0)
            {
                elapsedTime = 0;
            }
            return generatedEntities;
        }


        public Coconut CreateRandomCoconut()
        {
            Coconut coconut = new Coconut();
            GameManager gameManager = GameManager.Instance;
            coconut.texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.COCONUT_PATH);
            coconut.Init(gameManager.GameBounds);
            return coconut;
        }
    }
}
