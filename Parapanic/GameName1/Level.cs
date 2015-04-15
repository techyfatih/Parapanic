using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parapanic
{
    class Level
    {
        public virtual void Update()
        { }

        public virtual void Draw(SpriteBatch spriteBatch, Parapanic game)
        { }
    }
}
