using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong_Monogame
{
    abstract class Sprite
    {
        public Texture2D image;
        public Vector2 position;

        public Sprite(Texture2D image, Vector2 position)
        {
            this.image = image;
            this.position = position;
        }
    }
}
