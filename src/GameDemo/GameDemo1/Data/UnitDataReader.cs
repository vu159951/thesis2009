using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.DTO;
using System.Xml;

namespace GameDemo1.Data
{
    public class UnitDataReader : DataReader
    {
        public UnitDataReader()
        {
            xmlDoc = new XmlDocument();
        }
        public UnitDTO Load(String xmlFilePath)
        {
            UnitDTO unitDTO = new UnitDTO();
            xmlDoc.Load(xmlFilePath);
            return unitDTO;
        }
    }
}
