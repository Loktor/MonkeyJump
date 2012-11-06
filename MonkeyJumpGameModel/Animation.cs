using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyJumpGameModel
{
    public class Animation
    {
        public Texture2D Texture { get; set; }

        public Vector2 FrameSize { get; set; }

        public int FrameCount { get; set; }

        public int CurrentFrame { get; set; }

        public void Animation(Texture2D texture, int frameCount, Vector2 frameSize)
        {
            CurrentFrame = 0;
            Texture = texture;
            FrameCount = frameCount;
            FrameSize = frameSize;
        }

        public void Update()
        {

        }
    }
}
