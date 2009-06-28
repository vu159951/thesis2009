using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class UnitDTO: SpriteDTO
    {
        private Dictionary<String,ItemInfo> _informationList;
        private Dictionary<int,UpgradeInfo> _upgrades;

        internal Dictionary<String, ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }
        internal Dictionary<int, UpgradeInfo> Upgrades
        {
            get { return _upgrades; }
            set { _upgrades = value; }
        }

                

        public UnitDTO()
        {
            this._informationList = new Dictionary<string,ItemInfo>();
            this._upgrades = new Dictionary<int, UpgradeInfo>();
            this._action = new Dictionary<string,StatusInfo>();
        }
    }
}
