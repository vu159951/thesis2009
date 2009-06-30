using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameDemo1.DTO
{
    public class TerrainDTO: SpriteDTO
    {        
        public TerrainDTO()
        {            
            this._action = new Dictionary<string,StatusInfo>();
        }
    }
}
