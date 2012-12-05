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
using System.Runtime.Serialization;


namespace MonkeyJumpGameModel
{
    /// <summary>
    /// Highscore Entry
    /// </summary>
    [DataContractAttribute]
    public class HighscoreEntry
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public int Score { get; set; }

        /// <summary>
        /// For serialization
        /// </summary>
        public HighscoreEntry()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">PlayerName</param>
        /// <param name="score">PlayerScore</param>
        public HighscoreEntry(String name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
