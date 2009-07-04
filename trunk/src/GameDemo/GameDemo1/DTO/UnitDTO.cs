using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class UnitDTO: SpriteDTO
    {
        private Dictionary<String,ItemInfo> _informationList;// tập các thông tin liên quan đến 
        private Dictionary<int,UpgradeInfo> _upgrade;// tập các thông tin upgrade

        internal Dictionary<String, ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }
        internal Dictionary<int, UpgradeInfo> Upgrade
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
