
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
        //protected List<ItemInfo> _informationList;
        //protected List<List<ItemInfo>> _requirementList;
        //protected List<Info> _listUnits;
        //public List<Info> _listTechnology;    
        protected String _path;     
        #endregion

        #region Properties


        public virtual List<ItemInfo> InformationList
        {
            get { return null; }
            set { ; }
        }

        public virtual List<List<ItemInfo>> RequirementList
        {
            get { return null; }
            set { ; }
        }

        public virtual List<Info> UnitList
        {
            get { return null; }
            set {; }
        }

        public virtual List<Info> TechnologyList
        {
            get { return null; }
            set {; }
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

        public void CreateIcon(String desFolder)
        {
            try
            {
                _statusList[0].CreateIcon(desFolder);
            }
            catch (Exception) { }
        }

        public void ClearImageList()
        {
            foreach (SpriteStatus status in _statusList)
            {
                status.ClearImageList();
            }
        }

        public void ClearStatusList()
        {
            _statusList.Clear();
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
