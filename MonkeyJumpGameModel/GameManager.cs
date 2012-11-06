using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyJumpGameModel
{
    public class GameManager
    {
        private List<GameEntity> gameEntities;
        private LoopingBackground loopingBackground;
        private Player player;

        public Viewport Screen { get; set; }
        public int GameSpeed { get; set; }

        private static GameManager instance;

        private GameManager(Viewport screen) 
        {
            Screen = screen;
            GameSpeed = 5;
            gameEntities = new List<GameEntity>();
            loopingBackground = new LoopingBackground();
            player = new Player();

            gameEntities.Add(player);

        }

        public static GameManager CreateNewGameManager(Viewport screen)
        {
            instance = new GameManager(screen);
            return instance;
        }

        public static GameManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    throw new Exception("GameManager wasn't initialized yet, call InitializeGameManager before retreiving an instance");
                }
                return instance;
            }
        }
    
        public void UpdateEntities(GameTime gameTime)
        {
            loopingBackground.Update(gameTime);

            foreach(GameEntity entity in gameEntities)
            {
                entity.Update(gameTime);
            }
        }

        public void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime)
        {
            loopingBackground.Draw(spriteBatch,gameTime);

            foreach (GameEntity entity in gameEntities)
            {
                entity.Draw(spriteBatch,gameTime);
            }
        }

        public void LoadEntityTextures(ContentManager content)
        {
            loopingBackground.LoadTextures(content);

            foreach (GameEntity entity in gameEntities)
            {
                entity.LoadTextures(content);
            }
        }
    }
}
