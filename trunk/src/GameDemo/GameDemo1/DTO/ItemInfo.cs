using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class ItemInfo // lưu cặp giá trị name, value của 1 thẻ trong đặc tả
    {
        private string _name;  // lưu phần name
        private string _value; // lưu phần value

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
