using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyJumpGameModel
{
    public class LeafGenerator : GameEntityGenerator
    {
        int elapsedTime = 0;
        int generationTime = 3000;
        private Random rand = new Random(DateTime.Now.Millisecond);

        public override List<GameEntity> GenerateEntities(GameTime gameTime)
        {
            List<GameEntity> generatedEntities = new List<GameEntity>();
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            while (elapsedTime > generationTime)
            {
                elapsedTime -= generationTime;
                generatedEntities.Add(CreateRandomLeaf());
                generationTime = 3000 + rand.Next(2000);
            }
            if (generatedEntities.Count > 0)
            {
                elapsedTime = 0;
            }
            return generatedEntities;
        }


        public Leaf CreateRandomLeaf()
        {
            Leaf leaf = new Leaf();
            GameManager gameManager = GameManager.Instance;
            leaf.texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.LEAF_PATH);
            leaf.Init(gameManager.GameBounds);
            return leaf;
        }
    }
}
