using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.DTO
{
    public class ItemInfo : Item
    {
        #region Private Members

        private String _value;
 
        #endregion
     
        #region Properties

        public override String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public  String Value
        {
            get { return _value; }
            set { _value = value; }
        }     
    
        #endregion

        #region Contructors

        public ItemInfo()
        {
            _name = "";
            _value = "";
        }
        public ItemInfo(String name, String value)
        {
            _name = name;
            _value = value;
        }

        #endregion

        #region Public Methods

        public override String ToXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.SPEC_PATH
                );

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("Info")[0];
            String XMLString = node.OuterXml;

            XMLString = XMLString.Replace("%name%", _name);

            XMLString = XMLString.Replace("%value%", _value);

            return XMLString; 
        }
      
        #endregion
    }
}
