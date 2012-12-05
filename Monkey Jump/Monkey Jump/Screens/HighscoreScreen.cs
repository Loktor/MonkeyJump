using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonkeyJumpGameModel;


namespace Monkey_Jump.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class HighscoreScreen : MenuScreen
    {
        #region Fields

        List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HighscoreScreen()
            : base("Highscore")
        {
            // Create our menu entries.
            highscoreEntries = SaveGameManager.Instance.Highscore.HighscoreEntries.OrderByDescending(h => h.Score).ToList();

            HighscoreEntry currentEntry;

            // Add entries to the menu.
            for (int i = 0; i < highscoreEntries.Count; i++)
            {
                currentEntry = highscoreEntries.ElementAt(i);
                MenuEntries.Add(new MenuEntry(currentEntry.Name+" "+currentEntry.Score));
            }
        }

        #endregion
    }
}
