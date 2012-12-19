using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonkeyJumpGameModel;
using Microsoft.Xna.Framework.GamerServices;

namespace Monkey_Jump.Screens
{
    public delegate void GameOverClosingHandler(object sender, PlayerIndexEventArgs e);

    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    public class GameOverScreen : MenuScreen
    {
        #region Fields

        int score;
        Texture2D backgroundTexture;

        #endregion

        #region Events

        public event GameOverClosingHandler GameOverClosingPopup;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public GameOverScreen(int score)
            : base("Game Over")
        {
            this.score = score;

            if (SaveGameManager.Instance.Highscore.CheckIfNewHighscore(score))
            {
                SaveGameManager saveGameManager = SaveGameManager.Instance;
                IAsyncResult result = Guide.BeginShowKeyboardInput(0, "You made it in the highscorelist: " + score + " points", "Enter your name (max 10 letters):", saveGameManager.Options.LastTypedPlayerName == "" ? "Monkey" : SaveGameManager.Instance.Options.LastTypedPlayerName, GetTypedChars, null);
                while (!result.IsCompleted) ;
                saveGameManager.Highscore.AddScore(saveGameManager.Options.LastTypedPlayerName, score);
                saveGameManager.SaveHighscoreList();
            }

            // Create our menu entries.
            MenuEntry highScoreMenuEntry = new MenuEntry("Highscore");
            MenuEntry replayMenuEntry = new MenuEntry("Replay");

            // Hook up menu event handlers.
            highScoreMenuEntry.Selected += HighscoreMenuEntrySelected;
            replayMenuEntry.Selected += ReplayMenuEntrySelected;


            // Add entries to the menu.
            MenuEntries.Add(replayMenuEntry);
            MenuEntries.Add(highScoreMenuEntry);

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            backgroundTexture = content.Load<Texture2D>("backgroundGameOver");
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, new Rectangle(50, 50, ScreenManager.GraphicsDevice.Viewport.Width - 100, ScreenManager.GraphicsDevice.Viewport.Height - 100), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region Handle Input

        
        /// <summary>
        /// Event handler for when the Highscore menu entry is selected.
        /// </summary>
        void ReplayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
            GameOverClosingPopup(this, new PlayerIndexEventArgs(PlayerIndex.One));
            ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the Highscore menu entry is selected.
        /// </summary>
        void HighscoreMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighscoreScreen(true), e.PlayerIndex);
        }

        #endregion

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            base.OnCancel(playerIndex);
            GameOverClosingPopup(this, new PlayerIndexEventArgs(PlayerIndex.One));
            ScreenManager.AddScreen(new BackgroundScreen(), playerIndex);
            ScreenManager.AddScreen(new MainMenuScreen(), playerIndex);
        }

        protected void GetTypedChars(IAsyncResult r)
        {
            String enteredName = Guide.EndShowKeyboardInput(r);
            if (enteredName == null)
            {
                return;
            }
            if (enteredName.Length > 10)
            {
                SaveGameManager.Instance.Options.LastTypedPlayerName = enteredName.Substring(0, 10);
            }
            else
            {
                SaveGameManager.Instance.Options.LastTypedPlayerName = enteredName;
            }
            SaveGameManager.Instance.SaveOptions();
            ScreenManager.AddScreen(new HighscoreScreen(true), PlayerIndex.One);
        }
    }
}
