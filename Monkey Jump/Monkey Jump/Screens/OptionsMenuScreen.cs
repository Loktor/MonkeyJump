#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using MonkeyJumpGameModel;
#endregion

namespace Monkey_Jump
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry backgroundMusicMenuEntry;
        MenuEntry gameplaySoundMenuEntry;

        SaveGameManager saveGame = SaveGameManager.Instance;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            backgroundMusicMenuEntry = new MenuEntry(string.Empty);
            gameplaySoundMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            // Hook up menu event handlers.
            backgroundMusicMenuEntry.Selected += MusicMenuEntrySelected;
            gameplaySoundMenuEntry.Selected += GameplaySoundEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(backgroundMusicMenuEntry);
            MenuEntries.Add(gameplaySoundMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            gameplaySoundMenuEntry.Text = "Gameplay sounds: " + (saveGame.Options.GamePlaySoundsEnabled ? "on" : "off");
            backgroundMusicMenuEntry.Text = "Background music: " + (saveGame.Options.BackgroundMusicEnabled ? "on" : "off");
        }


        #endregion

        #region Handle Input



        void GameplaySoundEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            saveGame.Options.GamePlaySoundsEnabled = !saveGame.Options.GamePlaySoundsEnabled;

            saveGame.SaveOptions();

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            saveGame.Options.BackgroundMusicEnabled = !saveGame.Options.BackgroundMusicEnabled;

            saveGame.SaveOptions();

            SetMenuEntryText();
        }

        #endregion
    }
}
