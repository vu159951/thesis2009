using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSharedObject.DTO
{
    public class ItemInfo
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

        public ItemInfo()
        {
 
        }

        public ItemInfo(string name, string value)
        {
            this._name = name;
            this._value = value;
        }
    }
}
