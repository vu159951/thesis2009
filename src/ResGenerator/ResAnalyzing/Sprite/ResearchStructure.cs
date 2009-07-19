using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using ResAnalyzing.Sprite;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    class ResearchStructure : Structure
    {       
        #region Private Members

        //protected List<ItemInfo> _informationList;
        //protected List<List<ItemInfo>> _requirementList;
        private List<Info> _listTechnology;

        #endregion

        #region Properties

        public override List<Info> TechnologyList
        {
            get { return _listTechnology; }
            set { _listTechnology = value; }
        }

        #endregion

        #region Contructor
        public ResearchStructure()
        {
            _statusList = new List<SpriteStatus>();
            _informationList = new List<ItemInfo>();
            _requirementList = new List<List<ItemInfo>>();
            _listTechnology = new List<Info>();            

            _informationList.Add(new ItemInfo("", "MaxHealth", "400"));
            _informationList.Add(new ItemInfo("", "Power", "0"));
            _informationList.Add(new ItemInfo("", "RadiusAttack", "0"));
            _informationList.Add(new ItemInfo("", "RadiusDetect", "0"));
            _informationList.Add(new ItemInfo("", "Speed", "0"));

            List<ItemInfo> list1 = new List<ItemInfo>();
            list1.Add(new ItemInfo("", "Stone", "5000"));
            list1.Add(new ItemInfo("", "Gold", "4000"));
            list1.Add(new ItemInfo("", "Time", "3"));             
            list1.Add(new ItemInfo("Structure", "Logic1", "TownHall"));
            list1.Add(new ItemInfo("Structure", "Logic2", "Military"));
            _requirementList.Add(list1);

            _listTechnology.Add(new Info("Technology", "", "Brass_Metallurgy", "1"));
            _listTechnology.Add(new Info("Technology", "", "Iron_Metallurgy", "1"));
          
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

            String information = "", requirement = "", listTechnology = "";

            information = Utilities.GenXMLByList(InformationList);

            requirement = Utilities.GenXMLByList(RequirementList);

            listTechnology = Utilities.GenXMLByList(_listTechnology);

            String mainInfo = StatusList2XMLString();

            doc1.GetElementsByTagName("Informations")[0].RemoveChild(doc1.GetElementsByTagName("Informations")[0].FirstChild);
            doc1.GetElementsByTagName("Informations")[0].InnerXml = information;

            doc1.GetElementsByTagName("Requirements")[0].RemoveChild(doc1.GetElementsByTagName("Requirements")[0].FirstChild);
            doc1.GetElementsByTagName("Requirements")[0].InnerXml = requirement;

            doc1.GetElementsByTagName("ListTechnology")[0].RemoveChild(doc1.GetElementsByTagName("ListTechnology")[0].FirstChild);
            doc1.GetElementsByTagName("ListTechnology")[0].InnerXml = listTechnology;

            doc1.GetElementsByTagName("ListUnits")[0].InnerXml = "";

            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;

            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.RESEARCH_STRUCTURE);
            xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.STRUCTURE + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");


            return xml;
        }

        #endregion              
    }
}
