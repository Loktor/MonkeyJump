﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJumpGameModel
{
    /// <summary>
    /// Class to define an size
    /// </summary>
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
