using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class UnitDTO: SpriteDTO
    {        
        internal List<ItemInfo> Information
        {
            get { return _information; }
            set { _information = value; }
        }
        internal List<UpgradeInfo> Upgrades
        {
            get { return _upgrades; }
            set { _upgrades = value; }
        }

        private List<ItemInfo> _information;
        private List<UpgradeInfo> _upgrades;        

        public UnitDTO()
        {
            this._information = new List<ItemInfo>();
            this._upgrades = new List<UpgradeInfo>();
            this._action = new List<StatusInfo>();
        }
    }
}
