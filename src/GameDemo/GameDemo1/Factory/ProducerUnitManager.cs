using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Components;
using GameSharedObject.DTO;
using System.Reflection;
using System.IO;
using GameSharedObject.Data;
using Microsoft.Xna.Framework;
using System.CodeDom.Compiler;

namespace GameDemo1.Factory
{
    public class ProducerUnitManager : SpriteManager
    {
        public ProducerUnitManager(Game game)
            : base(game)
        {
            NS = "GameDemo1.Objects";
        }

        public override void Load()
        {
            base.Load();
        }
        public Sprite Add(string xmlPath, Vector2 position)
        {
            String noneParticle = "";
            return this.Add(xmlPath, noneParticle, position);
        }
        public override Sprite Add(String unitXmlPath, String particleSpecificationFile, Vector2 position)
        {
            codeGen.Load(GlobalDTO.OBJ_TEMPLATE_PATH + "ProducerUnit.cs");
            return base.Add(unitXmlPath, particleSpecificationFile, position);
        }
    }
}
