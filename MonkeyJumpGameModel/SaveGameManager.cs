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
        private const String OPTIONS_FILE = "options.dat";

        HighscoreList highscore = new HighscoreList();
        Options options = new Options();
        ObjectSerializer<HighscoreList> highscoreSerializer = new ObjectSerializer<HighscoreList>();
        ObjectSerializer<Options> optionsSerializer = new ObjectSerializer<Options>();
        private static SaveGameManager instance;

        public HighscoreList Highscore
        {
            get { return highscore; }
            set { highscore = value; }
        }

        public Options Options
        {
            get { return options; }
            set { options = value; }
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
        /// Saves all entries
        /// </summary>
        public void SaveAllEntries()
        {
            SaveHighscoreList();
            SaveOptions();
        }

        /// <summary>
        /// Loads all entries
        /// </summary>
        public void LoadAllEntries()
        {
            LoadHighscoreList();
            LoadOptions();
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
        /// Saves the current options
        /// </summary>
        public void SaveOptions()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication(); // grab the storage
            FileStream stream = store.OpenFile(OPTIONS_FILE, FileMode.Create); // Open a file in Create mode
            optionsSerializer.Serialize(stream, options);
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
            else
            {
                List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>() { new HighscoreEntry("Master Monkey", 7000), 
                    new HighscoreEntry("Angry Monkey", 4500), 
                    new HighscoreEntry("Jumping Monkey", 3000), 
                    new HighscoreEntry("Junior Monkey", 1500), 
                    new HighscoreEntry("Baby Monkey", 500) };
                highscore = new HighscoreList(highscoreEntries);
            }
        }

        /// <summary>
        /// Loads the options
        /// </summary>
        private void LoadOptions()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            if (store.FileExists(OPTIONS_FILE)) // Check if file exists
            {
                IsolatedStorageFileStream file = new IsolatedStorageFileStream(OPTIONS_FILE, FileMode.Open, store);
                options = optionsSerializer.Deserialize(file);
            }
            else
            {
                options = new Options(true,true);
            }
        }
    }
}
