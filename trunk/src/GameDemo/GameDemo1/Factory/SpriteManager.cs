using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Components;

namespace GameDemo1.Factory
{
    public abstract class SpriteManager : Dictionary<string, Sprite>
    {
        public abstract void Load();
        public abstract Sprite Add(string xmlPath);
    }
}
