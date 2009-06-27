using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class StructureDTO: SpriteDTO
    {        
        private List<ItemInfo> _informationList;
        private List<UpgradeInfo> _upgradeList;
        private List<ItemInfo> _unitList;

        public List<ItemInfo> UnitList
        {
            get { return _unitList; }
            set { _unitList = value; }
        }
        internal List<UpgradeInfo> UpgradeList
        {
            get { return _upgradeList; }
            set { _upgradeList = value; }
        }
        internal List<ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }

        public StructureDTO()
        {
            this._informationList = new List<ItemInfo>();
            this._upgradeList = new List<UpgradeInfo>();
            
        }
    }
}
