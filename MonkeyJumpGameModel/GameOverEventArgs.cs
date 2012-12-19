using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJumpGameModel
{
    public class GameOverEventArgs : EventArgs
    {
        public int Score { get; set; }

        public GameOverEventArgs(int score)
        {
            Score = score;
        }
    }
}
