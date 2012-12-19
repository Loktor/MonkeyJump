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
        int initTime = 1000;
        bool firstLeafGenerated = false;
        int generationTime = 3000;
        private Random rand = new Random(DateTime.Now.Millisecond);

        public override List<GameEntity> GenerateEntities(GameTime gameTime)
        {
            List<GameEntity> generatedEntities = new List<GameEntity>();
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (firstLeafGenerated == false)
            {
                if (elapsedTime > initTime)
                {
                    GenerateLeafWithRandomBanana(generatedEntities);
                    firstLeafGenerated = true;
                    elapsedTime = 0;
                }
                return generatedEntities;
            }
            while (elapsedTime > generationTime)
            {
                elapsedTime -= generationTime;
                GenerateLeafWithRandomBanana(generatedEntities);
                generationTime = (int)((4000 + rand.Next(3000)) / GameManager.Instance.GameSpeed);
            }
            if (generatedEntities.Count > 0)
            {
                elapsedTime = 0;
            }
            return generatedEntities;
        }

        private List<GameEntity> GenerateLeafWithRandomBanana(List<GameEntity> entityList)
        {
            entityList.Add(CreateRandomLeaf());
            if (rand.Next(2) == 0)
            {
                entityList.Add(CreateRandomBanana((Leaf)entityList[entityList.Count - 1]));
            }
            return entityList;
        }


        public Leaf CreateRandomLeaf()
        {
            Leaf leaf = new Leaf();
            GameManager gameManager = GameManager.Instance;
            leaf.texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.LEAF_PATH);
            leaf.Init(gameManager.GameBounds);
            return leaf;
        }

        public Banana CreateRandomBanana(Leaf leaf)
        {
            Banana banana = new Banana(leaf.Direction);
            GameManager gameManager = GameManager.Instance;
            banana.texture = gameManager.ResourceManager.RetreiveTexture(ResourceManager.BANANA_PATH);
            banana.Init(gameManager.GameBounds);

            banana.position.X = leaf.Direction == Direction.Left ? leaf.position.X - 30 + leaf.FrameSize.Width - 30 : leaf.position.X + 30;
            banana.position.Y -= 10; // to position the banana directly on the leave
            banana.SpriteEffects = leaf.SpriteEffects;
            return banana;
        }
    }
}