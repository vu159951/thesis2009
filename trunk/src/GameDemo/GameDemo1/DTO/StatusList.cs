using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class StatusList
    {
        public static StatusInfo IDLE = new StatusInfo(1, "IDLE");
        public static StatusInfo DEAD = new StatusInfo(2, "DEAD");
        public static StatusInfo MOVE = new StatusInfo(3, "MOVE");
        public static StatusInfo ATTACK = new StatusInfo(4, "ATTACK");
        public static StatusInfo HIT = new StatusInfo(5, "HIT");
        public static StatusInfo EXPLOIT = new StatusInfo(6, "EXPLOIT");        
    }
}
