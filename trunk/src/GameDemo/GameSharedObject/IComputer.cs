using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameSharedObject.DTO;

namespace GameSharedObject
{
    public interface IComputer:ICloneable
    {        
        void Init(int idlevel);
        void Play();
        void Clone(Game game);
    }
}
