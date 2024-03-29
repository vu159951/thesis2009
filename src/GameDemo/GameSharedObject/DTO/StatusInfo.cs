﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSharedObject.DTO
{
    public class StatusInfo
    {
        private Dictionary<String,DirectionInfo> _directionInfo;
        private int _id;
        private string _name;

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
        public Dictionary<String,DirectionInfo> DirectionInfo
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

        public StatusInfo(StatusInfo status)
        {
            this._id = status.Id;
            this._name = status.Name;
            this._directionInfo = status.DirectionInfo;
        }
    }
}
