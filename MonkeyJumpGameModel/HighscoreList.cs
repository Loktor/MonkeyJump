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
    /// Highscore Component
    /// </summary>
    [DataContractAttribute]
    public class HighscoreList
    {
        List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();

        [DataMember]
        public List<HighscoreEntry> HighscoreEntries
        {
            get { return highscoreEntries; }
            set { highscoreEntries = value; }
        }

        public HighscoreList()
        {
        }

        public HighscoreList(List<HighscoreEntry> highscoreList)
        {
            highscoreEntries = highscoreList;
        }

        /// <summary>
        /// Checks if the highscore is an top 5 score
        /// </summary>
        /// <param name="score">Score</param>
        /// <returns>true = yes, false = no</returns>
        public bool CheckIfNewHighscore(int score)
        {
            if (highscoreEntries.Count < 5)
            {
                return true;
            }
            for(int i = 0; i < highscoreEntries.Count; i++)
            {
                if(score > highscoreEntries.ElementAt(i).Score)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add an score and name to the highscore list, only gets added if its a top 5 highscore
        /// </summary>
        /// <param name="name">PlayerName</param>
        /// <param name="score">PlayerScore</param>
        public void AddScore(String name, int score)
        {
            if(highscoreEntries.Count < 5)
            {
                highscoreEntries.Add(new HighscoreEntry(name,score));
                return;
            }
            highscoreEntries = highscoreEntries.OrderBy(h => h.Score).ToList();
            highscoreEntries.RemoveAt(0);
            highscoreEntries.Add(new HighscoreEntry(name, score));
        }
    }
}
