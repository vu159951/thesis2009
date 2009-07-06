using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    class Structure : Sprite
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

        public override List<UnitInfo> ListUnits
        {
            get { return _listUnits; }
            set { _listUnits = value; }
        }

        #endregion

        #region Contructor
        public Structure()
        {
            _statusList = new List<SpriteStatus>();
            _informationList = new List<ItemInfo>();
            _requirementList = new List<List<ItemInfo>>();
            _listUnits = new List<UnitInfo>();
           
            _informationList.Add(new ItemInfo("MaxHealth", "80"));
            _informationList.Add(new ItemInfo("Power", "4"));
            _informationList.Add(new ItemInfo("RadiusAttack", "30"));
            _informationList.Add(new ItemInfo("RadiusDetect", "50"));
            _informationList.Add(new ItemInfo("Speed", "5"));

            List<ItemInfo> list1 = new List<ItemInfo>();
            list1.Add(new ItemInfo("Stone", "5000"));
            list1.Add(new ItemInfo("Gold", "4000"));
            list1.Add(new ItemInfo("Time", "3"));
            _requirementList.Add(list1);

            List<ItemInfo> list2 = new List<ItemInfo>();
            list2.Add(new ItemInfo("Stone", "10000"));
            list2.Add(new ItemInfo("Gold", "8000"));
            list2.Add(new ItemInfo("Time", "45"));
            _requirementList.Add(list2);

            _listUnits.Add(new UnitInfo("Black_Angel", "1"));
            _listUnits.Add(new UnitInfo("Archon_Archer", "1"));
            _listUnits.Add(new UnitInfo("Angel", "1"));
            _listUnits.Add(new UnitInfo("Elf_swordman", "2"));
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
                st._Status.Name = System.IO.Path.GetFileName(ls[i]);
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

            String information = "", requirement = "", listUnit = "";

            information = Utilities.GenXMLByList(InformationList);

            requirement = Utilities.GenXMLByList(RequirementList);

            listUnit = Utilities.GenXMLByList(_listUnits);

            String mainInfo = StatusList2XMLString();

            doc1.GetElementsByTagName("Information")[0].RemoveChild(doc1.GetElementsByTagName("Information")[0].FirstChild);
            doc1.GetElementsByTagName("Information")[0].InnerXml = information;

            doc1.GetElementsByTagName("Requirements")[0].RemoveChild(doc1.GetElementsByTagName("Requirements")[0].FirstChild);
            doc1.GetElementsByTagName("Requirements")[0].InnerXml = requirement;

            doc1.GetElementsByTagName("ListUnits")[0].RemoveChild(doc1.GetElementsByTagName("ListUnits")[0].FirstChild);
            doc1.GetElementsByTagName("ListUnits")[0].InnerXml = listUnit;

            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;

            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.STRUCTURE);
            xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.STRUCTURE + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");


            return xml;
        }

        #endregion              
    }
}
