using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonkeyJumpGameModel
{
    public class GameManager
    {
        private List<GameEntity> gameEntities;
        private LoopingBackground loopingBackground;
        private Player player;

        public Viewport Screen { get; set; }
        public int GameSpeed { get; set; }
        public Rectangle GameBounds { get; set; }

        private static GameManager instance;
        private const int BORDER_WIDTH = 60;

        private GameManager(Viewport screen) 
        {
            Rectangle tileSave = screen.TitleSafeArea;
            Screen = screen;
            GameSpeed = 5;
            // Move the GameBounds away from the sides because there are the palms
            GameBounds = new Rectangle(tileSave.X + BORDER_WIDTH, tileSave.Y, tileSave.Width - BORDER_WIDTH * 2, tileSave.Height);
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

        public Collider GameBoundsCollider
        {
            get
            {
                return new Collider(GameBounds);
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

        public void InitEntities()
        {
            loopingBackground.Init(GameBounds);

            foreach (GameEntity entity in gameEntities)
            {
                entity.Init(GameBounds);
            }
        }

        public void HandleInput(TouchCollection touch)
        {
            player.HandleInput(touch);
        }
    }
}
