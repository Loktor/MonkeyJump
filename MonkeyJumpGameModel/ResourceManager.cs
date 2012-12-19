using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace MonkeyJumpGameModel
{
    /// <summary>
    /// Class to store resources that need to be used more than one time
    /// </summary>
    public class ResourceManager
    {
        public const string BANANA_PATH = "game/banana";
        public const string BANANA_SCORE_PATH = "game/bananaScore";
        public const string LEAF_PATH = "game/leaf";
        public const string COCONUT_PATH = "game/coconut";
        public const string SHARK_PATH = "game/shark";
        public const string MONKEY_DEATH_SOUND = "game/monkeyDeath";
        public const string MONKEY_COLLECTABLE_SOUND = "game/monkeyCollectable";
        public const string MONKEY_BANANA_FALL = "game/monkeyBananaFall";
        public const string SCORE_FONT = "game/scoreFont";

        public const string WAVE_HIGH_LEFT_PATH = "game/highWavesLeft";
        public const string WAVE_HIGH_LEFT_BLOOD_PATH = "game/highWavesLeftBlood";
        public const string WAVE_HIGH_Right_PATH = "game/highWavesRight";
        public const string WAVE_HIGH_Right_BLOOD_PATH = "game/highWavesRightBlood";
        public const string WAVE_LOW_PATH = "game/lowWaves";
        public const string WAVE_LOW_BLOOD_PATH = "game/lowWavesBlood";

        Dictionary<String, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        Dictionary<String, SoundEffect> soundDictionary = new Dictionary<string, SoundEffect>();
        Dictionary<String, SpriteFont> fontDictionary = new Dictionary<string, SpriteFont>();

        private static ResourceManager instance;

        private ResourceManager() 
        {
        }

        /// <summary>
        /// Creates a new instance of the ResourceManager
        /// </summary>
        /// <returns></returns>
        public static ResourceManager CreateNewResourceManager()
        {
            instance = new ResourceManager();
            return instance;
        }

        /// <summary>
        /// Returns an instance of the saveGameManager
        /// </summary>
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    return CreateNewResourceManager();
                }
                return instance;
            }
        }



        public void Add(String key, Texture2D value)
        {
            if (textureDictionary.ContainsKey(key))
            {
                Debug.WriteLine("Key " + key + " is already in dictionary, resource refreshed");
                textureDictionary.Remove(key);
                textureDictionary.Add(key, value);
                return;
            }
            textureDictionary.Add(key, value);
        }

        public void Add(String key, SoundEffect value)
        {
            if (soundDictionary.ContainsKey(key))
            {
                Debug.WriteLine("Key " + key + " is already in dictionary, resource refreshed");
                soundDictionary.Remove(key);
                soundDictionary.Add(key, value);
                return;
            }
            soundDictionary.Add(key, value);
        }

        public void Add(String key, SpriteFont value)
        {
            if (fontDictionary.ContainsKey(key))
            {
                Debug.WriteLine("Key " + key + " is already in dictionary, resource refreshed");
                fontDictionary.Remove(key);
                fontDictionary.Add(key, value);
                return;
            }
            fontDictionary.Add(key, value);
        }

        public Texture2D RetreiveTexture(String key)
        {
            if (!textureDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key not in dictionary");
            }
            return textureDictionary[key];
        }

        public SoundEffect RetreiveSong(String key)
        {
            if (!soundDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key not in dictionary");
            }
            return soundDictionary[key];
        }

        public SpriteFont RetreiveFont(String key)
        {
            if (!fontDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key not in dictionary");
            }
            return fontDictionary[key];
        }
    }
}
