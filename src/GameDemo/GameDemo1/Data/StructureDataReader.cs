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
using GameDemo1.DTO;

namespace GameDemo1.Data
{
    public class StructureDataReader : DataReader
    {
        public StructureDataReader()
        {
            xmlDoc = new XmlDocument();
        }

        public StructureDTO Load(String xmlFilePath)
        {
            StructureDTO structureInfo = new StructureDTO();
            xmlDoc.Load(xmlFilePath);

            // name
            structureInfo.Name = xmlDoc.SelectSingleNode("//Sprite").Attributes["name"].Value;

            // information
            XmlNode nodeinfo = xmlDoc.SelectSingleNode("//Information");
            for (int i = 0; i < nodeinfo.ChildNodes.Count; i++)
            {
                ItemInfo info = new ItemInfo(nodeinfo.ChildNodes[i].Attributes["name"].Value, nodeinfo.ChildNodes[i].Attributes["value"].Value);
                structureInfo.InformationList.Add(info);
            }

            // upgrades 
            XmlNode noderequirement = xmlDoc.SelectSingleNode("//Requirements");
            for (int i = 0; i < noderequirement.ChildNodes.Count; i++)
            {
                XmlNode temp1 = noderequirement.ChildNodes[i];
                UpgradeInfo upgrade = new UpgradeInfo();
                for (int j = 0; j < temp1.ChildNodes.Count; j++)
                {
                    upgrade.Requirements.Add(new ItemInfo(temp1.ChildNodes[j].Attributes["name"].Value, temp1.ChildNodes[j].Attributes["value"].Value));
                }
                structureInfo.UpgradeList.Add(upgrade);
            }

            // list unit
            XmlNode nodeunits = xmlDoc.SelectSingleNode("//ListUnits");
            for (int i = 0; i < nodeunits.ChildNodes.Count; i++)
            {
                ItemInfo info = new ItemInfo(nodeunits.ChildNodes[i].Attributes["name"].Value, nodeunits.ChildNodes[i].Attributes["upgradeId"].Value);
                structureInfo.UnitList.Add(info);
            }


            // action
            XmlNode nodeAction = xmlDoc.SelectSingleNode("//Action");
            Game game = new Game();
            for (int i = 0; i < nodeAction.ChildNodes.Count; i++)
            {
                XmlNode temp1 = nodeAction.ChildNodes[i];
                StatusInfo statusinfo = new StatusInfo();
                for (int j = 0; j < temp1.ChildNodes.Count; j++)
                {
                    XmlNode temp2 = temp1.ChildNodes[j];
                    DirectionInfo directioninfo = new DirectionInfo();
                    for (int m = 0; m < temp2.ChildNodes.Count; m++)
                    {
                        directioninfo.Image.Add(game.Content.Load<Texture2D>(temp2.ChildNodes[m].Attributes["name"].Value));
                    }
                    statusinfo.DirectionInfo.Add(directioninfo);
                }
                structureInfo.Action.Add(statusinfo);
            }

            return structureInfo;
        }

    }
}
