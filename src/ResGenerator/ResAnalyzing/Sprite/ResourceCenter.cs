using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    class ResourceCenter : Sprite
    {
        #region Private Members
       
        #endregion

        #region Properties

        public override List<ItemInfo> InformationList
        {
            get { return _informationList; }
            set { _informationList = value; }
        }     
       
        #endregion

        #region Contructor
        public ResourceCenter()
        {
            _statusList = new List<SpriteStatus>();
            _informationList = new List<ItemInfo>();
            _informationList.Add(new ItemInfo("", "NameResource", "Stone"));
            _informationList.Add(new ItemInfo("", "Container", "40000"));        
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

            String information = "";        

            information = Utilities.GenXMLByList(InformationList);         

            String mainInfo = StatusList2XMLString();

            doc1.GetElementsByTagName("Informations")[0].RemoveChild(doc1.GetElementsByTagName("Informations")[0].FirstChild);
            doc1.GetElementsByTagName("Informations")[0].InnerXml = information;
            
            doc1.GetElementsByTagName("Requirements")[0].InnerXml = "";
            doc1.GetElementsByTagName("ListUnits")[0].InnerXml = "";

            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;



            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.RESOURCE_CENTER);
            xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.RESOURCE_CENTER + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");

            return xml;
        }

        #endregion       
    }
}
