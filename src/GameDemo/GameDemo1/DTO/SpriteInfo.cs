﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    class SpriteInfo
    {
        private string _name;
        private string _value;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        public SpriteInfo()
        {
 
        }

        public SpriteInfo(string name, string value)
        {
            this._name = name;
            this._value = value;
        }
    }
}
