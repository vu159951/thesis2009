
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.DTO
{
    public class ItemInfo
    {
        #region Private Members

        private String _type;
        private String _name;      
        private String _value;
 
        #endregion
     
        #region Properties

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public  String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion

        #region Contructors

        public ItemInfo()
        {
            _name = "";
            _value = "";
            _type = "";
        }
        public ItemInfo(String type, String name, String value)
        {
            _type = type;
            _name = name;
            _value = value;
        }

        #endregion

        #region Public Methods

        public String ToXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.SPEC_PATH
                );

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("Info")[0];
            String XMLString = node.OuterXml;

            XMLString = XMLString.Replace("%type%", _type);

            XMLString = XMLString.Replace("%name%", _name);

            XMLString = XMLString.Replace("%value%", _value);

            return XMLString; 
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
