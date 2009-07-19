using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.Sprite;
using ResAnalyzing.DTO;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    class Technology : Unit
    {
        #region Private Members

        #endregion

        #region Properties
       
        #endregion

        #region Contructor
        public Technology()
        {
            _statusList = new List<SpriteStatus>();
            _informationList = new List<ItemInfo>();
            _requirementList = new List<List<ItemInfo>>();

            List<ItemInfo> list = new List<ItemInfo>();
            _informationList.Add(new ItemInfo("", "", ""));

            list.Add(new ItemInfo("", "Stone", "3000"));
            list.Add(new ItemInfo("", "Gold", "5000"));
            list.Add(new ItemInfo("", "Time", "15"));
            _requirementList.Add(list);
        }
        #endregion

        #region Override Methods

        public override String ToXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.RULE_PATH);

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("Game")[0];

            XmlDocument doc1 = new XmlDocument();
            doc1.Load(Config.SPEC_PATH);

            String information = "", requirement = "";           

            information = Utilities.GenXMLByList(InformationList);

            requirement = Utilities.GenXMLByList(RequirementList);

            String mainInfo = StatusList2XMLString();

            doc1.GetElementsByTagName("Informations")[0].RemoveChild(doc1.GetElementsByTagName("Informations")[0].FirstChild);
            doc1.GetElementsByTagName("Informations")[0].InnerXml = information;

            doc1.GetElementsByTagName("Requirements")[0].RemoveChild(doc1.GetElementsByTagName("Requirements")[0].FirstChild);
            doc1.GetElementsByTagName("Requirements")[0].InnerXml = requirement;
          
            doc1.GetElementsByTagName("ListUnits")[0].InnerXml = "";
            doc1.GetElementsByTagName("ListTechnology")[0].InnerXml = "";

            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;



            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.TECHNOLOGY);
            xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.TECHNOLOGY + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");

            return xml;
        }

        #endregion       
    }
}
