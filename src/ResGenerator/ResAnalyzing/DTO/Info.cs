using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.DTO
{
    public class Info
    {
        #region Private Members

        private String _type;
        private String _upgradeId;
        private String _name;
        private String _tagName;

        #endregion
     
        #region Properties

        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public  String UpgradeId
        {
            get { return _upgradeId; }
            set { _upgradeId = value; }
        }

        public String TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }
    
        #endregion

        #region Contructors

        public Info()
        {
            _tagName = "Unit";
            _type = "";
            _name = "";
            _upgradeId = "";
        }
        public Info(String tagName, String type, String name, String upgradeId)
        {
            _tagName = tagName;
            _type = type;
            _name = name;
            _upgradeId = upgradeId;
        }

        #endregion

        #region Public Methods

        public String ToXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.SPEC_PATH);

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName(_tagName)[0];
            String XMLString = node.OuterXml;

            XMLString = XMLString.Replace("%type%", _type); 

            XMLString = XMLString.Replace("%name%", _name);

            XMLString = XMLString.Replace("%id%", _upgradeId);

            return XMLString; 
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
