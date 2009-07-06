using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    class SimpleSprite : Sprite
    {
    
        #region Private Members
       
        #endregion

        #region Properties
         
       
        #endregion

        #region Contructor
        public SimpleSprite()
        {
            _statusList = new List<SpriteStatus>();              
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

            String mainInfo = StatusList2XMLString();
           
            doc1.GetElementsByTagName("Action")[0].RemoveChild(doc1.GetElementsByTagName("Action")[0].FirstChild);
            doc1.GetElementsByTagName("Action")[0].InnerXml = mainInfo;

            doc1.GetElementsByTagName("Information")[0].InnerXml = "";
            doc1.GetElementsByTagName("Requirements")[0].InnerXml = "";
            doc1.GetElementsByTagName("ListUnits")[0].InnerXml = "";

            String xml = doc1.ChildNodes[0].OuterXml + doc1.ChildNodes[1].OuterXml;

            xml = xml.Replace("%foldername%", System.IO.Path.GetFileName(_path));
            xml = xml.Replace("%type%", Config.SIMPLE_SPRITE);
            if (Config.SIMPLE_SPRITE == "Particle")
            {
                xml = xml.Replace("%path%", Config.SIMPLE_SPRITE + "\\" + @"\"
                                + System.IO.Path.GetFileName(_path) + "\\" + @"\");
            }
            else
            {
                xml = xml.Replace("%path%", "Sprites" + "\\" + @"\" + Config.SIMPLE_SPRITE + "\\" + @"\");
            }
                             
            return xml;
        }

        #endregion       
    }
}
