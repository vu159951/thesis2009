using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class StatusInfo
    {
        private Dictionary<String,DirectionInfo> _directionInfo;// tập các hướng trong hành động
        private int _id;// mã của hành động
        private string _name; // tên hành động

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        internal Dictionary<String,DirectionInfo> DirectionInfo
        {
            get { return _directionInfo; }
            set { _directionInfo = value; }
        }

        public StatusInfo()
        {
            this._directionInfo = new Dictionary<string, DirectionInfo>();
        }
        public StatusInfo(int id, string name)
        {
            this._directionInfo = new Dictionary<string, DirectionInfo>();
            this._id = id;
            this._name = name;
        }
    }
}
