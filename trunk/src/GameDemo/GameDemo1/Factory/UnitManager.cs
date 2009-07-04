using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Components;
using GameSharedObject.DTO;
using System.Reflection;
using System.IO;
using GameSharedObject.Data;

namespace GameDemo1.Factory
{
    public class UnitManager: SpriteManager
    {
        public UnitManager()
        {
        }

        public override void Load()
        {
            string path = GlobalDTO.OBJ_UNIT_PATH;
            string[] pluginFiles = Directory.GetFiles(path, "*.DLL");
            Unit[] ipi = new Unit[pluginFiles.Length];

            for(int i= 0; i<pluginFiles.Length; i++){
                string args = Path.GetFileNameWithoutExtension(pluginFiles[i]);

                Type ObjType = null;
                try{
                    // load it
                    Assembly ass = null;
                    ass = Assembly.Load(args);
                    if (ass != null){
                        ObjType = ass.GetType(args+".Unit");
                    }
                }catch (Exception ex){
                    Logger.WriteLine(ex.Message);
                }
                try{
                    // OK Lets create the object as we have the Report Type
                    if (ObjType != null){
                        ipi[i] = (Unit)Activator.CreateInstance(ObjType);
                        // ipi[i].Host = this;
                        this.Add(ipi[i].Info.Name, ipi[i]);
                    }
                }catch (Exception ex){
                    Logger.WriteLine(ex.Message);
                }
            }
        }
        public override Sprite Add(String unitXmlPath)
        {
            return null;
        }
    }
}
