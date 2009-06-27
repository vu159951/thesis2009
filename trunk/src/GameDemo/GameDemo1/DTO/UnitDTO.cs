using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    class UnitDTO
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        internal List<SpriteInfo> Information
        {
            get { return _information; }
            set { _information = value; }
        }
        internal List<SpriteInfo> Requirement
        {
            get { return _requirement; }
            set { _requirement = value; }
        }
        internal List<StatusInfo> Action
        {
            get { return _action; }
            set { _action = value; }
        }

        List<SpriteInfo> _information;
        List<SpriteInfo> _requirement;
        List<StatusInfo> _action;

        public UnitDTO()
        {
            this._information = new List<SpriteInfo>();
            this._requirement = new List<SpriteInfo>();
            this._action = new List<StatusInfo>();
        }
    }
}
