using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.DTO
{
    public class ParticleDTO
    {
        private List<Texture2D> _image;        
        private string _name;

        public List<Texture2D> Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public ParticleDTO()
        {
            this._image = new List<Texture2D>();
            this._name = "";
        }
    }
}
