using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.DTO
{
    public class TechnologyDTO
    {        
        private UpgradeInfo _upgrade;
        private Texture2D _icon;
        private String _name;

        public UpgradeInfo Upgrade
        {
            get { return _upgrade; }
            set { _upgrade = value; }
        }
        public Texture2D Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public TechnologyDTO()
        {
            this._name = "";
        }
    }
}
