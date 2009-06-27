using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    class UpgradeInfo
    {
        private List<ItemInfo> _requirements;

        internal List<ItemInfo> Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }

        public UpgradeInfo()
        {
            this._requirements = new List<ItemInfo>();
        }
    }
}
