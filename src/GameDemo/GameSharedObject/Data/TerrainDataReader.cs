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
using System.Xml;
using GameSharedObject.DTO;

namespace GameSharedObject.Data
{
    public class TerrainDataReader : SpriteDataReader
    {
        public TerrainDataReader()
        {
            xmlDoc = new XmlDocument();
        }

        public override SpriteDTO Load(String xmlFilePath)
        {
            TerrainDTO terrainInfo = new TerrainDTO();
            xmlDoc.Load(xmlFilePath);

            // name
            terrainInfo.Name = xmlDoc.SelectSingleNode("//Sprite").Attributes["name"].Value;                                   

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
                        directioninfo.Image.Add( GlobalDTO.GAME.Content.Load<Texture2D>(path) );
                    }
                    directioninfo.Name = temp2.Name;
                    this.GetIdForDirection(directioninfo);
                    xmlDoc.Load(xmlFilePath);
                    statusinfo.DirectionInfo.Add(directioninfo.Name, directioninfo);
                }
                statusinfo.Name = temp1.Name;
                this.GetIdForAction(statusinfo);
                xmlDoc.Load(xmlFilePath);
                terrainInfo.Action.Add(statusinfo.Name, statusinfo);
            }            
            return (SpriteDTO)terrainInfo;
        }

    }
}
