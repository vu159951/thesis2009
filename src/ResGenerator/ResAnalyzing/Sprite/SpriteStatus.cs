using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using ResAnalyzing.DTO;

namespace ResAnalyzing.Sprite
{
    public class SpriteStatus
    {
        #region Private Members

        private String _path;
        private List<SpriteDirection> _directionList;      
        private Status _status;
        private int _startIndex;
        private String _folderName;

        #endregion

        #region Properties

        public String FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }
     
        public List<SpriteDirection> DirectionList
        {
            get { return _directionList; }           
        }
        #endregion

        #region Contructor
        public SpriteStatus()
        {
            _directionList = new List<SpriteDirection>();
            _status = new Status();
        }
        #endregion

        #region Public Methods
        public void Load(String folderPath)
        {
            String[] folder = Directory.GetDirectories(folderPath);
            List<String> ls = new List<String>();
            ls.AddRange(folder);
            ls.Sort();
                       
            for (int i = 0; i < ls.Count; i++)
            {               
                SpriteDirection dir = new SpriteDirection(_status);
                dir.Direction.Name = Utilities.GetTagName(System.IO.Path.GetFileName(ls[i]), "Direction");
                dir.Path = ls[i];
                dir.FolderName = FolderName;
                dir.Direction.Name = System.IO.Path.GetFileName(ls[i]);                              
                _directionList.Add(dir);
            }               
        }
        public void Load()
        {
            Load(Path);
        }
        public String ToXMLString()
        {           
            String xml = _status.ToXMLString();
            xml = xml.Replace("%s%", ToDirecttionListXMLString());
            return xml;
        }
        public override string ToString()
        {
            return base.ToString();
        }       
        public void MovenRenameImage(String desFolder)
        {
            foreach (SpriteDirection sDir in _directionList)
            {
                sDir.StartIndex = _startIndex;          
                sDir.Load();
                _startIndex = sDir.StartIndex;
                sDir.MovenRenameImage(desFolder);               
            }
        }

        public void CreateIcon(String desFolder)
        {
            try
            {
                _directionList[0].CreateIcon(desFolder);
            }
            catch (Exception) { }
        }       

        public void ClearImageList()
        {

            foreach (SpriteDirection sDir in _directionList)
            {
                sDir.ClearImageList();
            }
        }       

        #endregion

        #region Private Methods

        private String ToDirecttionListXMLString()
        {
            String xml = "";
            foreach (SpriteDirection sDir in _directionList)
            {
                sDir.StartIndex = _startIndex;               
                sDir.Load();
                _startIndex = sDir.StartIndex;
                xml += sDir.ToXMLString();
            }

            return xml;
        }

        #endregion
    }
}
