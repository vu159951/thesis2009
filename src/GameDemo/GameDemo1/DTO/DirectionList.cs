using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class DirectionList // list các hướng
    {
        public static DirectionInfo S = new DirectionInfo(1, "S");
        public static DirectionInfo ES = new DirectionInfo(2, "ES");
        public static DirectionInfo E = new DirectionInfo(3, "E");
        public static DirectionInfo EN = new DirectionInfo(4, "EN");
        public static DirectionInfo N = new DirectionInfo(5, "N");
        public static DirectionInfo WN = new DirectionInfo(6, "WN");
        public static DirectionInfo W = new DirectionInfo(7, "W");
        public static DirectionInfo WS = new DirectionInfo(8, "WS");        
    }
}
