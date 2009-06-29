using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GameDemo1.DTO;

namespace GameDemo1.Data
{
    public abstract class DataReader
    {
        protected XmlDocument xmlDoc = new XmlDocument();

        public void GetIdForAction(StatusInfo statusinfo)
        {
            xmlDoc.Load(GameDemo1.Properties.Settings.Default.GlobalFile);
            foreach (XmlNode direction in xmlDoc.SelectSingleNode("//Action").ChildNodes)
            {
                if (direction.Attributes["tagName"].Value == statusinfo.Name)
                {
                    statusinfo.Id = int.Parse(direction.Attributes["id"].Value);
                }
            }            
        }

        public void GetIdForDirection(DirectionInfo directioninfo)
        {            
            xmlDoc.Load(GameDemo1.Properties.Settings.Default.GlobalFile);
            foreach(XmlNode direction in xmlDoc.SelectSingleNode("//Direction").ChildNodes)
            {
                if (direction.Attributes["tagName"].Value == directioninfo.Name)
                {
                    directioninfo.Id = int.Parse(direction.Attributes["id"].Value);
                    break;
                }
            }
        }   
    }
}
