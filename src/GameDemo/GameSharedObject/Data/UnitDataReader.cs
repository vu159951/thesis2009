﻿using System;
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
using GameSharedObject.DTO;
using System.Xml;

namespace GameSharedObject.Data
{
    public class UnitDataReader : SpriteDataReader
    {

        public UnitDataReader()
        {
            xmlDoc = new XmlDocument();            
        }

        public override SpriteDTO Load(String xmlFilePath)
        {
            UnitDTO unitDTO = new UnitDTO();
            xmlDoc.Load(xmlFilePath);

            // name
            unitDTO.Name = xmlDoc.SelectSingleNode("//Sprite").Attributes["name"].Value;

            // information
            XmlNode nodeinfo = xmlDoc.SelectSingleNode("//Informations");
            for (int i = 0; i < nodeinfo.ChildNodes.Count; i++)
            {
                ItemInfo info = new ItemInfo(nodeinfo.ChildNodes[i].Attributes["name"].Value, nodeinfo.ChildNodes[i].Attributes["value"].Value, nodeinfo.ChildNodes[i].Attributes["type"].Value);
                unitDTO.InformationList.Add(info.Name, info);
            }

            // upgrades 
            XmlNode noderequirement = xmlDoc.SelectSingleNode("//Requirements");
            for (int i = 0; i < noderequirement.ChildNodes.Count; i++)
            {
                XmlNode temp1 = noderequirement.ChildNodes[i];
                UpgradeInfo upgrade = new UpgradeInfo();
                for (int j = 0; j < temp1.ChildNodes.Count; j++)
                {
                    upgrade.Requirements.Add(temp1.ChildNodes[j].Attributes["name"].Value, new ItemInfo(temp1.ChildNodes[j].Attributes["name"].Value, temp1.ChildNodes[j].Attributes["value"].Value, temp1.ChildNodes[j].Attributes["type"].Value));
                }
                upgrade.Id = int.Parse(temp1.Attributes["id"].Value);
                upgrade.Name = temp1.Attributes["name"].Value;
                unitDTO.Upgrade.Add(upgrade.Id, upgrade);
            }

            // action
            XmlNode nodeAction = xmlDoc.SelectSingleNode("//Action");
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
                        string path = GlobalDTO.RES_CONTENT_PATH + xmlDoc.SelectSingleNode("//Sprite").Attributes["path"].Value + temp2.ChildNodes[m].Attributes["name"].Value;
                        path = System.IO.Path.GetFullPath(path);
                        directioninfo.Image.Add(GlobalDTO.GAME.Content.Load<Texture2D>(path));
                    }
                    directioninfo.Name = temp2.Name;
                    this.GetIdForDirection(directioninfo);
                    xmlDoc.Load(xmlFilePath);
                    statusinfo.DirectionInfo.Add(directioninfo.Name, directioninfo);
                }
                statusinfo.Name = temp1.Name;
                this.GetIdForAction(statusinfo);
                xmlDoc.Load(xmlFilePath);
                unitDTO.Action.Add(statusinfo.Name, statusinfo);
            }

            // icon 
            string p = GlobalDTO.RES_CONTENT_PATH + xmlDoc.SelectSingleNode("//Sprite").Attributes["path"].Value;
            p = System.IO.Path.GetFullPath(p);
            unitDTO.Icon = GlobalDTO.GAME.Content.Load<Texture2D>(p + "Icon");

            return (SpriteDTO)unitDTO;
        }
    }
}
