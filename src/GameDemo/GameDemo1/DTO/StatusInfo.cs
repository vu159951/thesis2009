using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class StatusInfo
    {
        List<DirectionInfo> _directionInfo;

        internal List<DirectionInfo> DirectionInfo
        {
            get { return _directionInfo; }
            set { _directionInfo = value; }
        }

        public StatusInfo()
        {
            this._directionInfo = new List<DirectionInfo>();
        }
    }
}
