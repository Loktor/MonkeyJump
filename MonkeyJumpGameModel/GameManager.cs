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
        private List<GameEntity> collidableGameEntities;
        private List<GameEntity> decorationEntities;
        private List<GameEntityGenerator> gameEntityGenerators;
        private LoopingBackground loopingBackground;
        private Player player;

        public Viewport Screen { get; set; }
        public int GameSpeed { get; set; }
        public Rectangle GameBounds { get; set; }
        public ResourceManager ResourceManager { get; set; }

        private static GameManager instance;
        private const int BORDER_WIDTH = 60;
#if DEBUG
        private bool showBounds = true;
        private const String BOUNDS_RECT_TEX_KEY = "debug/rect";
#endif

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
                return new Collider(GameBounds, false);
            }
        }

        private GameManager(Viewport screen) 
        {
            ResourceManager = new ResourceManager();
            Rectangle tileSave = screen.TitleSafeArea;
            Screen = screen;
            GameSpeed = 5;
            // Move the GameBounds away from the sides because there are the palms
            GameBounds = new Rectangle(tileSave.X + BORDER_WIDTH, tileSave.Y, tileSave.Width - BORDER_WIDTH * 2, tileSave.Height);
            collidableGameEntities = new List<GameEntity>();
            decorationEntities = new List<GameEntity>();
            loopingBackground = new LoopingBackground();

            player = new Player();

            collidableGameEntities.Add(player);

            InitGameEntityGenerators();
        }

        private void InitGameEntityGenerators()
        {
            gameEntityGenerators = new List<GameEntityGenerator>();
            gameEntityGenerators.Add(new CoconutGenerator());
        }
    
        public void UpdateEntities(GameTime gameTime)
        {
            List<GameEntity> entitiesToRemove = new List<GameEntity>();
            loopingBackground.Update(gameTime);

            foreach(GameEntity entity in collidableGameEntities)
            {
                entity.Update(gameTime);
                if (!((ICollidable)entity).Collider.CollidesWith(GameBoundsCollider))
                {
                    entitiesToRemove.Add(entity);
                }
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.Update(gameTime);
            }
            
            foreach (GameEntityGenerator generator in gameEntityGenerators)
            {
                collidableGameEntities.AddRange(generator.GenerateEntities(gameTime));
            }

            foreach (GameEntity entityToRemove in entitiesToRemove)
            {
                collidableGameEntities.Remove(entityToRemove);
            }
        }

        public void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime)
        {
            loopingBackground.Draw(spriteBatch,gameTime);

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.Draw(spriteBatch,gameTime);
#if DEBUG
                if (showBounds)
                {
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(BOUNDS_RECT_TEX_KEY), ((ICollidable)entity).Collider.CollisionBounds, Color.White);
                }
#endif
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }

        public void LoadEntityTextures(ContentManager content)
        {
#if DEBUG
            if (showBounds)
            {
                ResourceManager.Add(BOUNDS_RECT_TEX_KEY,content.Load<Texture2D>(BOUNDS_RECT_TEX_KEY));
            }
#endif
            loopingBackground.LoadTextures(content);

            // Load resuable textures
            ResourceManager.Add(ResourceManager.COCONUT_PATH,content.Load<Texture2D>(ResourceManager.COCONUT_PATH));

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.LoadTextures(content);
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.LoadTextures(content);
            }
        }

        public void InitEntities()
        {
            loopingBackground.Init(GameBounds);

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.Init(GameBounds);
            }
            foreach (GameEntity entity in decorationEntities)
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
