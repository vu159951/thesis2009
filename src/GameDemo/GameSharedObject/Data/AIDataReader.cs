using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GameSharedObject.DTO;


namespace GameSharedObject.Data
{
    class AIDataReader
    {
        private XmlDocument xmlDoc;

        public AIDataReader()
        {
            xmlDoc = new XmlDocument();
        }

        public AIDTO Load(String pathAIXml, int level)
        {
            AIDTO ai = new AIDTO();
            this.xmlDoc.Load(pathAIXml);
            ai.Id = level;
            XmlNode nodeLevel = xmlDoc.SelectSingleNode("//Level[@id=" + level + "]");
            ai.NameLevel = nodeLevel.Attributes["name"].Value;

            for (int i = 0; i < nodeLevel.ChildNodes.Count; i++)
            {
                ai.Actions.Add(nodeLevel.ChildNodes[i].Attributes["name"].Value, new ItemInfo(nodeLevel.ChildNodes[i].Attributes["name"].Value, nodeLevel.ChildNodes[i].Attributes["value"].Value, nodeLevel.ChildNodes[i].Attributes["type"].Value));
            }
            return ai;
        }
    }
}
