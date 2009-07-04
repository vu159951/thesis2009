﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameDemo1.DTO
{
    public abstract class SpriteDTO
    {
        protected string _name;// tên sprite
        protected Texture2D _icon; // hình icon đại diện
        protected Dictionary<String, StatusInfo> _action; // tập các hành động

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
