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
        protected XmlDocument xmlDoc;

        public void GetIdForAction(StatusInfo statusinfo, string name)
        {
            if (name == "IDLE")
            {
                statusinfo.Id = 1;
            }
            else if (name == "DEAD")
            {
                statusinfo.Id = 2;
            }
            else if (name == "MOVE")
            {
                statusinfo.Id = 3;
            }
            else if (name == "ATTACK")
            {
                statusinfo.Id = 4;
            }
            else if (name == "HIT")
            {
                statusinfo.Id = 5;
            }
            else if (name == "EXPLOIT")
            {
                statusinfo.Id = 6;
            }            
        }

        public void GetIdForDirection(DirectionInfo directioninfo, string name)
        {
            if (name == "S")
            {
                directioninfo.Id = 1;
            }
            else if (name == "ES")
            {
                directioninfo.Id = 2;
            }
            else if (name == "E")
            {
                directioninfo.Id = 3;
            }
            else if (name == "EN")
            {
                directioninfo.Id = 4;
            }
            else if (name == "N")
            {
                directioninfo.Id = 5;
            }
            else if (name == "WN")
            {
                directioninfo.Id = 6;
            }
            else if (name == "W")
            {
                directioninfo.Id = 7;
            }
            else if (name == "WS")
            {
                directioninfo.Id = 8;
            }
        }   
    }
}
