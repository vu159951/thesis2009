
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using ResAnalyzing.DTO;

namespace ResAnalyzing.Sprite
{    
    public abstract class Sprite
    {
        #region Private Members

        protected List<SpriteStatus> _statusList;
        protected List<ItemInfo> _informationList;
        protected List<List<ItemInfo>> _requirementList;
        protected List<UnitInfo> _listUnits;
        protected String _path;     
        #endregion

        #region Properties
            

        public virtual List<ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }

        public virtual List<List<ItemInfo>> RequirementList
        {
            get { return _requirementList; }
            set { _requirementList = value; }
        }

        public virtual List<UnitInfo> ListUnits
        {
            get { return _listUnits; }
            set { _listUnits = value; }
        }

        #endregion

        #region Contructor
       
        #endregion

        #region Virtual Methods

        public virtual String ToXMLString()
        {
            return "";
        }

        public virtual void Load(String folderPath)
        {
        }

        #endregion

        #region Public Methods
       
        public override string ToString()
        {
            return base.ToString();
        }       
        public void MovenRenameImage(String desFolder)
        {
            int startIndex = 0;
            foreach (SpriteStatus status in _statusList)
            {
                status.StartIndex = startIndex;             
                if (status.DirectionList.Count == 0)
                {
                    status.Load();
                }
                status.MovenRenameImage(desFolder);
                startIndex = status.StartIndex;
            }
        }
        #endregion

        #region Protected Methods
        protected String StatusList2XMLString()
        {
            String xml = "";
            int startIndex = 0;
            foreach (SpriteStatus status in _statusList)
            {
                status.StartIndex = startIndex;                            
                if (status.DirectionList.Count == 0)
                {
                    status.Load();
                }               
                xml += status.ToXMLString();
                startIndex = status.StartIndex;
            }

            return xml;
        }
        #endregion
    }
}
