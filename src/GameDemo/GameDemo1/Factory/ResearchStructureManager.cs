﻿using System;
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
    public class ResearchStructureManager : SpriteManager
    {
        public ResearchStructureManager(Game game) : base(game) { }

        public override Sprite Add(String unitXmlPath, String ObjSpritePath, String SpecSpritePath)
        {
            attrList.Clear();
            codeGen.Load(GlobalDTO.OBJ_TEMPLATE_PATH + "ResearchStructure.cs");
            reader = new StructureDataReader();
            attrList.Add("this.PercentSize", "1.25f");
            return base.Add(unitXmlPath, ObjSpritePath, SpecSpritePath);
        }
        /// <summary>
        /// Hàm chính thực hiện việc load
        /// </summary>
        /// <param name="spriteName">Tên của đối tượng cần load</param>
        /// <param name="ObjSpritePath">Đường dẫn chứa tập tin có mã IL chứa tập tin cần load</param>
        /// <param name="SpecSpritePath">Đường dẫn chưa tập tin đặc tả</param>
        /// <returns>Đối tượng được load lên</returns>
        public override Sprite Load(String spriteName, String ObjSpritePath, String SpecSpritePath)
        {
            Sprite sprite =  base.Load(spriteName, ObjSpritePath, SpecSpritePath);
            if (!(sprite is ResearchStructure))
            {
                this.Remove(sprite.Info.Name);
                return null;
            }
            return sprite;
        }        
    }
}
