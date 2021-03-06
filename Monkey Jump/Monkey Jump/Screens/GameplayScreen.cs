﻿#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonkeyJumpGameModel;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using Monkey_Jump.Screens;
#endregion

namespace Monkey_Jump
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        GameOverScreen gameOverScreen;
        ContentManager content;
        SpriteFont gameFont;
        GameManager gameManager;
        Song bgMusic;

        Random random = new Random();

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            if (gameManager == null)
                gameManager = GameManager.CreateNewGameManager(ScreenManager.GraphicsDevice.Viewport);

            gameFont = content.Load<SpriteFont>("gamefont");

            gameManager.GameOver += new GameOverEventHandler(HandleGameOver);
            gameManager.LoadEntityTextures(content);

            gameManager.InitEntities();

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            if (SaveGameManager.Instance.Options.BackgroundMusicEnabled)
            {
                bgMusic = content.Load<Song>("game/bgSound");
                MediaPlayer.IsRepeating = true;

                MediaPlayer.Play(bgMusic);
            }
        }

        void HandleGameOver(object sender, GameOverEventArgs e)
        {
            gameOverScreen = new GameOverScreen(e.Score);
            ScreenManager.AddScreen(gameOverScreen, PlayerIndex.One);
            gameOverScreen.DisplayNameKeyboard();
            gameOverScreen.GameOverClosingPopup += new GameOverClosingHandler(GameOverPopupClosing);
        }

        void GameOverPopupClosing(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                gameManager.UpdateEntities(gameTime);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
            TouchCollection touchState = input.TouchState;

            // if the user pressed the back button, we return to the main menu
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                MediaPlayer.Stop();
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
            }
            else
            {
                gameManager.HandleInput(touchState);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            gameManager.DrawEntities(spriteBatch, gameTime);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);
        }


        #endregion
    }
}
