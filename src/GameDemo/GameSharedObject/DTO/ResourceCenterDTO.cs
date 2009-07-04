using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameSharedObject.DTO
{
    public class ResourceCenterDTO: SpriteDTO
    {
        private Dictionary<String, ItemInfo> _resourceInfo;
        public Dictionary<String, ItemInfo> ResourceInfo
        {
            get { return _resourceInfo; }
            set { _resourceInfo = value; }
        }

        public ResourceCenterDTO()
        {            
            this._action = new Dictionary<string,StatusInfo>();
            this._resourceInfo = new Dictionary<string, ItemInfo>();
        }
    }
}
