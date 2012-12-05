﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyJumpGameModel
{
    public class GameManager
    {
        private List<GameEntity> collidableGameEntities;
        private List<GameEntity> decorationEntities;
        private List<GameEntityGenerator> gameEntityGenerators;
        private LoopingBackground loopingBackground;
        private LoopingWaves loopingWavesRight;
        private LoopingWaves loopingWavesLeft;
        private LoopingWaves loopingWavesLow;
        private Player player;
        private Shark shark;
        private GameState gameState = GameState.NotCreated;

        private static GameManager instance;
        private SaveGameManager saveGameManager;
        private const int BORDER_WIDTH = 60;
        private Vector2 scorePos = new Vector2(300, 10);
        private const String SCORE_TEXT = "Score: ";

        private int elapsedTime = 0;
        private int increaseGameSpeedThreshold = 10000;
#if DEBUG
        private bool showBounds = true;
        private const String BOUNDS_RECT_TEX_KEY = "debug/rect";
#endif

        public Viewport Screen { get; set; }
        public float GameSpeed { get; set; }
        public Rectangle GameBounds { get; set; }
        public ResourceManager ResourceManager { get; set; }

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
            saveGameManager = SaveGameManager.Instance;
            Screen = screen;
            GameSpeed = 1;
            // Move the GameBounds away from the sides because there are the palms
            GameBounds = new Rectangle(tileSave.X + BORDER_WIDTH, tileSave.Y-200, tileSave.Width - BORDER_WIDTH * 2, tileSave.Height+200);
            collidableGameEntities = new List<GameEntity>();
            decorationEntities = new List<GameEntity>();
            loopingBackground = new LoopingBackground();
            loopingWavesRight = new LoopingWaves(true, false);
            loopingWavesLeft = new LoopingWaves(false, false);
            loopingWavesLow = new LoopingWaves(false, true);

            player = new Player();
            shark = new Shark();
            decorationEntities.Add(shark);

            InitGameEntityGenerators();
            gameState = GameState.Running;
        }

        private void InitGameEntityGenerators()
        {
            gameEntityGenerators = new List<GameEntityGenerator>();
            gameEntityGenerators.Add(new CoconutGenerator());
            gameEntityGenerators.Add(new LeafGenerator());
        }
    
        public void UpdateEntities(GameTime gameTime)
        {
            if (gameState == GameState.Paused || gameState == GameState.Over)
            {
                return;
            }
            if (player.PlayerState == PlayerState.Dead)
            {
                if (saveGameManager.Highscore.CheckIfNewHighscore(player.PlayerScore))
                {
                    saveGameManager.Highscore.AddScore("test", player.PlayerScore);
                    saveGameManager.SaveHighscoreList();
                    gameState = GameState.Over;
                }
                return;
            }

            List<GameEntity> entitiesToRemove = new List<GameEntity>();
            loopingBackground.Update(gameTime);
            loopingWavesRight.Update(gameTime);
            loopingWavesLeft.Update(gameTime);
            loopingWavesLow.Update(gameTime);

            foreach(GameEntity entity in collidableGameEntities)
            {
                entity.Update(gameTime);
                if (!((ICollidable)entity).Collider.CollidesWith(GameBoundsCollider))
                {
                    entitiesToRemove.Add(entity);
                }
                else if (player.Collider.CollidesWith(((ICollidable)entity).Collider))
                {
                    if(entity is ICollectable){

                        entitiesToRemove.Add(entity);

                        SoundEffect collectSound = ResourceManager.RetreiveSong(ResourceManager.MONKEY_COLLECTABLE_SOUND);
                        collectSound.Play();

                        player.PlayerScore += (entity as ICollectable).Score;                        
                    }else{
                        player.KillPlayer();
                    }
                }
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.Update(gameTime);
            }

            player.Update(gameTime);

            foreach (GameEntityGenerator generator in gameEntityGenerators)
            {
                collidableGameEntities.AddRange(generator.GenerateEntities(gameTime));
            }

            foreach (GameEntity entityToRemove in entitiesToRemove)
            {
                collidableGameEntities.Remove(entityToRemove);
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            UpdateGameSpeed(elapsedTime);
        }

        public void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime)
        {
            loopingWavesLeft.Draw(spriteBatch, gameTime);
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

            player.Draw(spriteBatch, gameTime);
#if DEBUG
            if (showBounds)
            {
                spriteBatch.Draw(ResourceManager.RetreiveTexture(BOUNDS_RECT_TEX_KEY), ((ICollidable)player).Collider.CollisionBounds, Color.White);
            }
#endif

            loopingWavesRight.Draw(spriteBatch, gameTime);

            foreach (GameEntity entity in decorationEntities)
            {
                entity.Draw(spriteBatch, gameTime);
            }

            loopingWavesLow.Draw(spriteBatch, gameTime);
            spriteBatch.DrawString(ResourceManager.RetreiveFont(ResourceManager.SCORE_FONT), SCORE_TEXT + player.PlayerScore, scorePos, Color.GhostWhite);
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
            loopingWavesRight.LoadTextures(content);
            loopingWavesLeft.LoadTextures(content);
            loopingWavesLow.LoadTextures(content);


            // Load resuable textures
            ResourceManager.Add(ResourceManager.MONKEY_DEATH_SOUND, content.Load<SoundEffect>(ResourceManager.MONKEY_DEATH_SOUND));
            ResourceManager.Add(ResourceManager.MONKEY_COLLECTABLE_SOUND, content.Load<SoundEffect>(ResourceManager.MONKEY_COLLECTABLE_SOUND));
            ResourceManager.Add(ResourceManager.BANANA_PATH, content.Load<Texture2D>(ResourceManager.BANANA_PATH));
            ResourceManager.Add(ResourceManager.LEAF_PATH, content.Load<Texture2D>(ResourceManager.LEAF_PATH));
            ResourceManager.Add(ResourceManager.COCONUT_PATH, content.Load<Texture2D>(ResourceManager.COCONUT_PATH));
            ResourceManager.Add(ResourceManager.SHARK_PATH, content.Load<Texture2D>(ResourceManager.SHARK_PATH));
            ResourceManager.Add(ResourceManager.SCORE_FONT, content.Load<SpriteFont>(ResourceManager.SCORE_FONT));

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.LoadTextures(content);
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.LoadTextures(content);
            }

            player.LoadTextures(content);
        }

        public void InitEntities()
        {
            loopingBackground.Init(GameBounds);
            loopingWavesRight.Init(GameBounds);
            loopingWavesLeft.Init(GameBounds);
            loopingWavesLow.Init(GameBounds);

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.Init(GameBounds);
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.Init(GameBounds);
            }
            player.Init(GameBounds);
        }

        public void HandleInput(TouchCollection touch)
        {
            player.HandleInput(touch);
        }

        private void UpdateGameSpeed(int elapsedTime)
        {
            if (elapsedTime > increaseGameSpeedThreshold)
            {
                if (GameSpeed < 5)
                {
                    GameSpeed *= 1.1f;
                }
                elapsedTime = 0;
            }
        }
    }
}
