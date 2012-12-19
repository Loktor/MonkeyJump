using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;

namespace MonkeyJumpGameModel
{
    public class GameManager
    {
        private List<GameEntity> collidableGameEntities;
        private List<GameEntity> decorationEntities;
        private List<GameEntityGenerator> gameEntityGenerators;
        private Texture2D mainBackground;
        private Vector2 bgPosition = new Vector2(0, -200);
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

        private SpriteFont scoreFont;
        private Vector2 scorePos = new Vector2(410, 10);
        private const String SCORE_TEXT = "Score: ";

        private Vector2 bananaScorePos1 = new Vector2(400, 50);
        private Vector2 bananaScorePos2 = new Vector2(370, 50);
        private Vector2 bananaScorePos3 = new Vector2(340, 50);

        private int elapsedTimeSinceSpeedUpdate = 0;
        private int increaseGameSpeedThreshold = 10000;
#if DEBUG
        private bool showBounds = false;
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
            ResourceManager = ResourceManager.Instance;
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
            gameEntityGenerators.Add(new LeafGenerator());
            gameEntityGenerators.Add(new CoconutGenerator());
        }
    
        public void UpdateEntities(GameTime gameTime)
        {
            if (gameState == GameState.Paused || gameState == GameState.Over)
            {
                return;
            }
            if (player.PlayerState == PlayerState.Dead)
            {
                loopingWavesLeft.setBloodTexture();
                loopingWavesLow.setBloodTexture();
                loopingWavesRight.setBloodTexture();

                if (saveGameManager.Highscore.CheckIfNewHighscore(player.PlayerScore))
                {
                    saveGameManager.Highscore.AddScore("test", player.PlayerScore);
                    saveGameManager.SaveHighscoreList();
                    gameState = GameState.Over;

                    
                    //Guide.BeginShowKeyboardInput(0, "Enter Name:", "Name for the highscore.", "Test Player", null, null);
                }
                return;
            }

            List<GameEntity> entitiesToRemove = new List<GameEntity>();
            loopingBackground.Update(gameTime);
            loopingWavesRight.Update(gameTime);
            loopingWavesLeft.Update(gameTime);
            loopingWavesLow.Update(gameTime);

            if (bgPosition.Y < -65)
                bgPosition.Y += 0.1f;


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

                        SoundPlayer.Instance.PlaySound(ResourceManager.MONKEY_COLLECTABLE_SOUND);

                        player.PlayerScore += (entity as ICollectable).Score;
                        if(player.BananaScore < 3)          
                            player.BananaScore++;

                    }else{
                        if (player.BananaScore == 3)
                        {
                            player.BananaScore = 0;
                            player.StartImmortality();

                            SoundPlayer.Instance.PlaySound(ResourceManager.MONKEY_BANANA_FALL);

                            BananaFallDown bfd = new BananaFallDown();
                            bfd.Init(GameBounds, player.position);
                            decorationEntities.Add(bfd);

                        }
                        else if(!player.IsImmortal)
                        {
                            player.KillPlayer();
                        }
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

            elapsedTimeSinceSpeedUpdate += gameTime.ElapsedGameTime.Milliseconds;

            UpdateGameSpeed();
        }

        public void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(mainBackground, bgPosition, Color.White);
            loopingWavesLeft.Draw(spriteBatch, gameTime);
            loopingBackground.Draw(spriteBatch,gameTime);

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.Draw(spriteBatch,gameTime);
                DrawDebugOutput(spriteBatch, entity);
            }

            player.Draw(spriteBatch, gameTime);
            DrawDebugOutput(spriteBatch, player);

            loopingWavesRight.Draw(spriteBatch, gameTime);

            foreach (GameEntity entity in decorationEntities)
            {
                entity.Draw(spriteBatch, gameTime);
            }

            loopingWavesLow.Draw(spriteBatch, gameTime);


            // Draw the Score
            String scoreTxt = SCORE_TEXT + player.PlayerScore;
            Vector2 size = scoreFont.MeasureString( scoreTxt );

            Vector2 scorePosCurr = scorePos;
            scorePosCurr.X -= size.X;
            spriteBatch.DrawString(ResourceManager.RetreiveFont(ResourceManager.SCORE_FONT), scoreTxt, scorePosCurr, Color.GhostWhite);
        

            // Draw the already collected bananas.
            switch (player.BananaScore)
            {
                case 3:
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos1, Color.White);
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos2, Color.White);
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos3, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos1, Color.White);
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos2, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(ResourceManager.RetreiveTexture(ResourceManager.BANANA_SCORE_PATH), bananaScorePos1, Color.White);
                    break;

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
            mainBackground = content.Load<Texture2D>("game/backgroundGame");
            loopingBackground.LoadTextures(content);
            loopingWavesRight.LoadTextures(content);
            loopingWavesLeft.LoadTextures(content);
            loopingWavesLow.LoadTextures(content);
            

            // Load resuable textures
            ResourceManager.Add(ResourceManager.MONKEY_DEATH_SOUND, content.Load<SoundEffect>(ResourceManager.MONKEY_DEATH_SOUND));
            ResourceManager.Add(ResourceManager.MONKEY_COLLECTABLE_SOUND, content.Load<SoundEffect>(ResourceManager.MONKEY_COLLECTABLE_SOUND));
            ResourceManager.Add(ResourceManager.MONKEY_BANANA_FALL, content.Load<SoundEffect>(ResourceManager.MONKEY_BANANA_FALL));
            ResourceManager.Add(ResourceManager.BANANA_PATH, content.Load<Texture2D>(ResourceManager.BANANA_PATH));
            ResourceManager.Add(ResourceManager.BANANA_SCORE_PATH, content.Load<Texture2D>(ResourceManager.BANANA_SCORE_PATH));
            ResourceManager.Add(ResourceManager.LEAF_PATH, content.Load<Texture2D>(ResourceManager.LEAF_PATH));
            ResourceManager.Add(ResourceManager.COCONUT_PATH, content.Load<Texture2D>(ResourceManager.COCONUT_PATH));
            ResourceManager.Add(ResourceManager.SHARK_PATH, content.Load<Texture2D>(ResourceManager.SHARK_PATH));
            ResourceManager.Add(ResourceManager.SCORE_FONT, content.Load<SpriteFont>(ResourceManager.SCORE_FONT));

            ResourceManager.Add(ResourceManager.WAVE_HIGH_LEFT_BLOOD_PATH, content.Load<Texture2D>(ResourceManager.WAVE_HIGH_LEFT_BLOOD_PATH));
            ResourceManager.Add(ResourceManager.WAVE_HIGH_Right_BLOOD_PATH, content.Load<Texture2D>(ResourceManager.WAVE_HIGH_Right_BLOOD_PATH));
            ResourceManager.Add(ResourceManager.WAVE_LOW_BLOOD_PATH, content.Load<Texture2D>(ResourceManager.WAVE_LOW_BLOOD_PATH));

            foreach (GameEntity entity in collidableGameEntities)
            {
                entity.LoadTextures(content);
            }
            foreach (GameEntity entity in decorationEntities)
            {
                entity.LoadTextures(content);
            }

            player.LoadTextures(content);
            scoreFont = ResourceManager.RetreiveFont(ResourceManager.SCORE_FONT);
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

        private void UpdateGameSpeed()
        {
            if (elapsedTimeSinceSpeedUpdate > increaseGameSpeedThreshold)
            {
                if (GameSpeed < 5)
                {
                    GameSpeed += 0.2f;
                }
                elapsedTimeSinceSpeedUpdate = 0;
            }
        }

        private void DrawDebugOutput(SpriteBatch spriteBatch, GameEntity entity)
        {
#if DEBUG
            if (showBounds && entity is ICollidable)
            {
                Collider col = ((ICollidable)entity).Collider;
                Texture2D boundsRect = ResourceManager.RetreiveTexture(BOUNDS_RECT_TEX_KEY);
                spriteBatch.Draw(boundsRect, col.CollisionBounds, Color.White);
                if (col is MultiCollider)
                {
                    foreach (Collider mCol in ((MultiCollider)col).Colliders)
                    {
                        spriteBatch.Draw(boundsRect, mCol.CollisionBounds, Color.White);
                    }
                }
            }
#endif
        }
    }
}
