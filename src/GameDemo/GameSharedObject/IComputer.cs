using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameSharedObject.DTO;

namespace GameSharedObject
{
    public interface IComputer
    {        
        void Init();
        void Play();
    }
}
