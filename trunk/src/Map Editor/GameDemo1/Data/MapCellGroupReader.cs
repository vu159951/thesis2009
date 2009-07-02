using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.DTO;
using System.Xml;

namespace GameDemo1.Data
{
    public class MapCellGroupReader
    {
        public static MapCellGroupCollection Read(string xmlPath)
        {
            MapCellGroupCollection result = new MapCellGroupCollection();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlElement root = xmlDoc.DocumentElement;

            result.Quantity = int.Parse(root.Attributes["quantity"].Value);
            foreach(XmlElement e in root.ChildNodes){
                MapCellGroup group = new MapCellGroup();
                group.Name = e.Attributes["name"].Value;
                group.Id = int.Parse(e.Attributes["id"].Value);
                group.StartIndex = int.Parse(e.Attributes["start"].Value);
                group.EndIndex = int.Parse(e.Attributes["end"].Value);
                result.Add(group.Id, group);
            }

            return result;
        }
    }

    
}
