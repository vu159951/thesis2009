using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using System.Xml;

namespace ResAnalyzing.Sprite 
{
    class Unit : Sprite
    {
        #region Private Members      
       
        #endregion

        #region Properties

        public override List<ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }

        public override List<List<ItemInfo>> RequirementList
        {
            get { return _requirementList; }
            set { _requirementList = value; }
        }      
       
        #endregion

        #region Contructor
        public Unit()
        {
            _statusList = new List<SpriteStatus>();
            _informationList = new List<ItemInfo>();
            _requirementList = new List<List<ItemInfo>>();

            List<ItemInfo> list = new List<ItemInfo>();
            _informationList.Add(new ItemInfo("", "MaxHealth", "150"));
            _informationList.Add(new ItemInfo("", "Power", "3"));
            _informationList.Add(new ItemInfo("", "RadiusAttack", "40"));
            _informationList.Add(new ItemInfo("", "RadiusDetect", "60"));
            _informationList.Add(new ItemInfo("", "Speed", "5"));
            _informationList.Add(new ItemInfo("", "AttackParticle", "Boiling Fury Fire"));

            list.Add(new ItemInfo("", "Gold", "40"));
            list.Add(new ItemInfo("", "Time", "16"));
            _requirementList.Add(list);
        }
        #endregion

        #region Override Methods

        public override void Load(string folderPath)
        {           
            _path = folderPath;
            String[] folder = System.IO.Directory.GetDirectories(folderPath);
            List<String> ls = new List<String>();
            ls.AddRange(folder);

            for (int i = 0; i < ls.Count; i++)
            {
                SpriteStatus st = new SpriteStatus();
                st.Path = ls[i];
                st.FolderName = System.IO.Path.GetFileName(folderPath);
                st.Status.Name = System.IO.Path.GetFileName(ls[i]);
                _statusList.Add(st);
            }                  
        }

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

            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;



            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.UNIT);
            xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.UNIT + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");

            return xml;
        }

        #endregion       
    }
}
