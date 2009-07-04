using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSharedObject.DTO
{
    public class UpgradeInfo
    {
        private Dictionary<String,ItemInfo> _requirements;
        private string _name;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Dictionary<String,ItemInfo> Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }

        public UpgradeInfo()
        {
            this._requirements = new Dictionary<string, ItemInfo>();
        }
    }
}
