using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSharedObject.DTO
{
    class AIDTO
    {
        private String _nameLevel;
        private int _id;
        private Dictionary<String, ItemInfo> _actions;

        public Dictionary<String, ItemInfo> Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public String NameLevel
        {
            get { return _nameLevel; }
            set { _nameLevel = value; }
        }


        public AIDTO()
        {
            this._nameLevel = "";
            this._id = 0;
            this._actions = new Dictionary<string, ItemInfo>();
        }        
    }
}
