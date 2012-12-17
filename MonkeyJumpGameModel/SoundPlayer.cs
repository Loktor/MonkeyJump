using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyJumpGameModel
{
    public class SoundPlayer
    {
        private static SoundPlayer instance;

        private SoundPlayer() 
        {
            
        }

        /// <summary>
        /// Creates a new instance of the saveGameManager
        /// </summary>
        /// <returns></returns>
        public static SoundPlayer CreateNewSoundPlayer()
        {
            instance = new SoundPlayer();
            return instance;
        }

        /// <summary>
        /// Returns an instance of the saveGameManager
        /// </summary>
        public static SoundPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    return CreateNewSoundPlayer();
                }
                return instance;
            }
        }

        /// <summary>
        /// Plays an sound with the specified key
        /// </summary>
        /// <param name="soundKey">String key</param>
        public void PlaySound(String soundKey)
        {
            if (SaveGameManager.Instance.Options.GamePlaySoundsEnabled)
            {
                ResourceManager.Instance.RetreiveSong(soundKey).Play();
            }
        }
    }
}
