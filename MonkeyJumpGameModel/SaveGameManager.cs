using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace MonkeyJumpGameModel
{
    public class SaveGameManager
    {
        private const String HIGHSCORE_FILE = "highscore.dat";

        HighscoreList highscore = new HighscoreList();
        ObjectSerializer<HighscoreList> highscoreSerializer = new ObjectSerializer<HighscoreList>();
        private static SaveGameManager instance;

        public HighscoreList Highscore
        {
            get { return highscore; }
            set { highscore = value; }
        }

        private SaveGameManager() 
        {
        }

        /// <summary>
        /// Creates a new instance of the saveGameManager
        /// </summary>
        /// <returns></returns>
        public static SaveGameManager CreateNewSaveGameManager()
        {
            instance = new SaveGameManager();
            return instance;
        }

        /// <summary>
        /// Returns an instance of the saveGameManager
        /// </summary>
        public static SaveGameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    return CreateNewSaveGameManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Saves the current highscore list
        /// </summary>
        public void SaveHighscoreList()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication(); // grab the storage
            FileStream stream = store.OpenFile(HIGHSCORE_FILE, FileMode.Create); // Open a file in Create mode
            highscoreSerializer.Serialize(stream, highscore);
            stream.Close();
        }

        /// <summary>
        /// Loads the highscore list
        /// </summary>
        public void LoadHighscoreList()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            if (store.FileExists(HIGHSCORE_FILE)) // Check if file exists
            {
                IsolatedStorageFileStream file = new IsolatedStorageFileStream(HIGHSCORE_FILE, FileMode.Open, store);
                highscore = highscoreSerializer.Deserialize(file);
            }
        }
    }
}
