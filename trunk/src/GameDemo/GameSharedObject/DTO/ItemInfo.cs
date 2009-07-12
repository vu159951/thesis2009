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
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

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

        public ItemInfo(string name, string value,string type)
        {
            this._name = name;
            this._value = value;
            this._type = type;
        }
    }
}
