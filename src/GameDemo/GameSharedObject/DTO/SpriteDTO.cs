using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.DTO
{
    public abstract class SpriteDTO
    {
        protected string _name;
        protected Texture2D _icon;
        protected Dictionary<String, StatusInfo> _action;

        public Texture2D Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Dictionary<String, StatusInfo> Action
        {
            get { return _action; }
            set { _action = value; }
        }
    }
}
