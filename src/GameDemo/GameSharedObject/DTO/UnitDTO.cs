using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameSharedObject.DTO
{
    public class UnitDTO: SpriteDTO
    {
        private Dictionary<String,ItemInfo> _informationList;
        private Dictionary<int,UpgradeInfo> _upgrade;

        public Dictionary<String, ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }
        public Dictionary<int, UpgradeInfo> Upgrade
        {
            get { return _upgrade; }
            set { _upgrade = value; }
        }

                

        public UnitDTO()
        {
            this._informationList = new Dictionary<string,ItemInfo>();
            this._upgrade = new Dictionary<int, UpgradeInfo>();
            this._action = new Dictionary<string,StatusInfo>();
        }
    }
}
