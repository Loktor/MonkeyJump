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
        Texture2D backgroundTexture;
        ContentManager content;
        bool redrawBackground;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HighscoreScreen(bool redrawBackground)
            : base("Highscore")
        {
            this.redrawBackground = redrawBackground;

            // Create our menu entries.
            highscoreEntries = SaveGameManager.Instance.Highscore.HighscoreEntries.OrderByDescending(h => h.Score).ToList();

            HighscoreEntry currentEntry;

            if (highscoreEntries.Count == 0)
            {
                base.MenuTitle = "No highscores yet";
            }

            // Add entries to the menu.
            for (int i = 0; i < highscoreEntries.Count; i++)
            {
                currentEntry = highscoreEntries.ElementAt(i);
                MenuEntries.Add(new MenuEntry(currentEntry.Name+" "+currentEntry.Score));
            }
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (redrawBackground)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                backgroundTexture = content.Load<Texture2D>("game/background");
            }
            base.LoadContent();
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            if (redrawBackground)
            {
                content.Unload();
            }
            base.UnloadContent();
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            if (redrawBackground)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

                spriteBatch.Begin();

                spriteBatch.Draw(backgroundTexture, fullscreen,
                                 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
