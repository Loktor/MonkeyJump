using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MonkeyJumpGameModel
{
    /// <summary>
    /// Class to store resources that need to be used more than one time
    /// </summary>
    public class ResourceManager
    {
        public const string COCONUT_PATH = "game/coconut";
        public const string MONKEY_DEATH_SOUND = "game/monkeyDeath";
        public const string SCORE_FONT = "game/scoreFont";

        Dictionary<String, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        Dictionary<String, Song> songDictionary = new Dictionary<string, Song>();
        Dictionary<String, SpriteFont> fontDictionary = new Dictionary<string, SpriteFont>();

        public void Add(String key, Texture2D value)
        {
            if (textureDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key is already in dictionary");
            }
            textureDictionary.Add(key, value);
        }

        public void Add(String key, Song value)
        {
            if (songDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key is already in dictionary");
            }
            songDictionary.Add(key, value);
        }

        public void Add(String key, SpriteFont value)
        {
            if (fontDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key is already in dictionary");
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

        public Song RetreiveSong(String key)
        {
            if (!songDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException("Key not in dictionary");
            }
            return songDictionary[key];
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
