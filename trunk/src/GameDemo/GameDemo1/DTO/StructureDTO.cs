﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class StructureDTO: SpriteDTO
    {
        private Dictionary<String, ItemInfo> _informationList;// tập các thông tin liên quan đến Structure, health, power ...
        private Dictionary<int, UpgradeInfo> _upgradeList; // tập thông tin Upgrade
        private List<ItemInfo> _unitList; // tập các Unit có thể mua

        public List<ItemInfo> UnitList
        {
            get { return _unitList; }
            set { _unitList = value; }
        }
        public Dictionary<int,UpgradeInfo> UpgradeList
        {
            get { return _upgradeList; }
            set { _upgradeList = value; }
        }
        public Dictionary<String,ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }

        public StructureDTO()
        {
            this._informationList = new Dictionary<string, ItemInfo>();
            this._upgradeList = new Dictionary<int, UpgradeInfo>();
            this._unitList = new List<ItemInfo>();
            this._action = new Dictionary<string, StatusInfo>();
        }
    }
}
