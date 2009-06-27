using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDemo1.DTO
{
    class DirectionInfo
    {
        List<Texture2D> _image;
        public List<Texture2D> Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public DirectionInfo()
        {
            this._image = new List<Texture2D>();
        }
    }
}
