using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public abstract class SpriteDTO
    {
        protected string _name;
        protected List<StatusInfo> _action;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<StatusInfo> Action
        {
            get { return _action; }
            set { _action = value; }
        }
    }
}
