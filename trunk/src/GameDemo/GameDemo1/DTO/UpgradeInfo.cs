using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class UpgradeInfo
    {
        private Dictionary<String,ItemInfo> _requirements; // thông tin yêu cầu cho upgrade
        private string _name;// tên upgrade
        private int _id; // mã số

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
