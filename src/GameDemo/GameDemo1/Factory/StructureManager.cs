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
    public class StructureManager : SpriteManager
    {
        public StructureManager(Game game) : base(game) { }

        public override Sprite Add(String unitXmlPath, String ObjSpritePath, String SpecSpritePath)
        {
            attrList.Clear();
            codeGen.Load(GlobalDTO.OBJ_TEMPLATE_PATH + "Structure.cs");
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
            return base.Load(spriteName, ObjSpritePath, SpecSpritePath);
        }
        public List<Structure> ToArray()
        {
            List<Structure> ls = new List<Structure>();

            foreach (KeyValuePair<string, Sprite> item in this){
                ls.Add((Structure)item.Value);
            }
            return ls;
        }
    }
}
