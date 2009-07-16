using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using GameSharedObject.DTO;

namespace GameSharedObject.Data
{
    public class TechnologyDataReader
    {
        private XmlDocument xmlDoc;
        public TechnologyDataReader()
        {
            xmlDoc = new XmlDocument();            
        }        

        public TechnologyDTO Load(String xmlFilePath)
        {
            TechnologyDTO tech = new TechnologyDTO();
            xmlDoc.Load(xmlFilePath);

            // name
            tech.Name = xmlDoc.SelectSingleNode("//Sprite").Attributes["name"].Value;

            // requirement
            XmlNode noderequirement = xmlDoc.SelectSingleNode("//Requirements");
            for (int i = 0; i < noderequirement.ChildNodes.Count; i++)
            {
                XmlNode temp1 = noderequirement.ChildNodes[i];
                UpgradeInfo upgrade = new UpgradeInfo();
                for (int j = 0; j < temp1.ChildNodes.Count; j++)
                {
                    upgrade.Requirements.Add(temp1.ChildNodes[j].Attributes["name"].Value, new ItemInfo(temp1.ChildNodes[j].Attributes["name"].Value, temp1.ChildNodes[j].Attributes["value"].Value, temp1.ChildNodes[j].Attributes["type"].Value));
                }
                upgrade.Name = temp1.Attributes["name"].Value;
                upgrade.Id = int.Parse(temp1.Attributes["id"].Value);
                tech.Upgrade = upgrade;
            }
            // icon 
            string p = GlobalDTO.RES_CONTENT_PATH + xmlDoc.SelectSingleNode("//Sprite").Attributes["path"].Value;
            p = System.IO.Path.GetFullPath(p);
            tech.Icon = GlobalDTO.GAME.Content.Load<Texture2D>(p + "Icon");

            return tech;
        }

    }
}
