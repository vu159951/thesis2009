using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.DTO
{
    public class UnitInfo : Item
    {
        #region Private Members

        private String _upgradeId;
             
        #endregion
     
        #region Properties

        public override String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public  String UpgradeId
        {
            get { return _upgradeId; }
            set { _upgradeId = value; }
        }  
    
        #endregion

        #region Contructors

        public UnitInfo()
        {
            _name = "";
            _upgradeId = "";
        }
        public UnitInfo(String name, String upgradeId)
        {
            _name = name;
            _upgradeId = upgradeId;
        }

        #endregion

        #region Public Methods

        public override String ToXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.SPEC_PATH);

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("Unit")[0];
            String XMLString = node.OuterXml;

            XMLString = XMLString.Replace("%name%", _name);

            XMLString = XMLString.Replace("%id%", _upgradeId);

            return XMLString; 
        }    

        #endregion
    }
}
