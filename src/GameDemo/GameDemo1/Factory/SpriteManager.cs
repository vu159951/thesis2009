using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Components;
using Microsoft.Xna.Framework;
using GameSharedObject.DTO;
using System.Reflection;
using GameSharedObject.Data;
using System.IO;
using System.CodeDom.Compiler;

namespace GameDemo1.Factory
{
    public abstract class SpriteManager : Dictionary<string, Sprite>
    {
        protected Game _game;
        protected String EXTENSION = ".dll";
        protected CodeCompiler compiler;
        protected CodeGenerator codeGen;
        protected String NS;

        public SpriteManager(Game game){
            _game = game;
            CodeGenerator codeGen = new CodeGenerator();
            compiler = new CodeCompiler();

            NS = codeGen.GetHostNamespace() + ".Objects";            
            compiler.AddReference("System.dll");
            compiler.AddReference("System.Data.dll");
            compiler.AddReference(GlobalDTO.XNA_FRAMEWORK_PATH + "Microsoft.Xna.Framework.dll");
            compiler.AddReference(GlobalDTO.XNA_FRAMEWORK_PATH + "Microsoft.Xna.Framework.Game.dll");
            compiler.AddReference("GameSharedObject.dll");
        }
        public virtual void Load()
        {
            string path = GlobalDTO.OBJ_UNIT_PATH;
            string[] pluginFiles = Directory.GetFiles(path, "*.DLL");
            Unit[] ipi = new Unit[pluginFiles.Length];

            for (int i = 0; i < pluginFiles.Length; i++)
            {
                string dllFile = Path.GetFileNameWithoutExtension(pluginFiles[i]);

                Type ObjType = null;
                try{
                    // load it
                    Assembly asm = null;
                    asm = Assembly.Load(dllFile);
                    if (asm != null)
                    {
                        ObjType = asm.GetType(NS);
                    }
                }
                catch (Exception ex){
                    Logger.WriteLine(ex.Message);
                }
                try{
                    // OK Lets create the object as we have the Report Type
                    if (ObjType != null)
                    {
                        ipi[i] = (Unit)Activator.CreateInstance(ObjType);
                        // ipi[i].Host = this;
                        this.Add(ipi[i].Info.Name, ipi[i]);
                    }
                }
                catch (Exception ex){
                    Logger.WriteLine(ex.Message);
                }
                this.Add(ipi[i].Info.Name, ipi[i]);
            }
        }
        public virtual Sprite Add(String unitXmlPath, String particleSpecificationFile, Vector2 position){
            // GENERATE code
            UnitDataReader rd = new UnitDataReader();
            String spriteName = ((UnitDTO)rd.Load(unitXmlPath)).Name;
            Dictionary<String, String> attrList = new Dictionary<String, String>();

            attrList.Add("this.PercentSize", "0.5f");
            codeGen.AddAttrDeclaration(attrList);
            String code = codeGen.Process(codeGen.GetHostNamespace(), spriteName);

            // BUILD code - Run the compiler and build the assembly
            String dllFile = GlobalDTO.OBJ_UNIT_PATH + spriteName + this.EXTENSION;
            CompilerResults result = compiler.Compile(code, dllFile);
            if (result.Errors.HasErrors){
                Logger.Clear();
                foreach(String s in result.Output){
                    Logger.WriteLine(s + Environment.NewLine);
                }
                throw new Exception("Error! Cannot build the object.");
            }

            // LOAD object - Load the generated assembly into the ApplicationDomain 
            Assembly asm = Assembly.LoadFrom(dllFile);
            Type t = asm.GetType(NS + "." + spriteName);
            // BindingFlags enumeration specifies flags that control binding and 
            // the way in which the search for members and types is conducted by reflection. 
            // The following specifies the Access Control of the bound type
            BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance;
            // Construct an instance of the type and invoke the member method
            int playerId = 0;   // The object depend on none-player
            Object obj = t.InvokeMember(spriteName, bflags |
                BindingFlags.CreateInstance, null, null,
                new object[]{
                    _game, 
                    unitXmlPath, 
                    particleSpecificationFile, 
                    position, 
                    playerId}
                );

            // Copy specification file to the owner folder
            String path = GlobalDTO.SPEC_UNIT_PATH + Path.GetFileName(unitXmlPath);
            try { File.Copy(unitXmlPath, path, true); }
            catch { }

            return (Sprite)obj;
        }
        public virtual Sprite AddExt(String unitXmlPath, object[] parameters)
        {
            // GENERATE code
            UnitDataReader rd = new UnitDataReader();
            String spriteName = ((UnitDTO)rd.Load(unitXmlPath)).Name;
            Dictionary<String, String> attrList = new Dictionary<String, String>();

            attrList.Add("this.PercentSize", "0.5f");
            codeGen.AddAttrDeclaration(attrList);
            String code = codeGen.Process(codeGen.GetHostNamespace(), spriteName);

            // BUILD code - Run the compiler and build the assembly
            String dllFile = GlobalDTO.OBJ_UNIT_PATH + spriteName + this.EXTENSION;
            CompilerResults result = compiler.Compile(code, dllFile);
            if (result.Errors.HasErrors)
            {
                Logger.Clear();
                foreach (String s in result.Output)
                {
                    Logger.WriteLine(s + Environment.NewLine);
                }
                throw new Exception("Error! Cannot build the object.");
            }

            // LOAD object - Load the generated assembly into the ApplicationDomain 
            Assembly asm = Assembly.LoadFrom(dllFile);
            Type t = asm.GetType(NS + "." + spriteName);
            // BindingFlags enumeration specifies flags that control binding and 
            // the way in which the search for members and types is conducted by reflection. 
            // The following specifies the Access Control of the bound type
            BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance;
            // Construct an instance of the type and invoke the member method
            int playerId = 0;   // The object depend on none-player
            Object obj = t.InvokeMember(spriteName, bflags |
                BindingFlags.CreateInstance, null, null,
                parameters
                );

            // Copy specification file to the owner folder
            String path = GlobalDTO.SPEC_UNIT_PATH + Path.GetFileName(unitXmlPath);
            try { File.Copy(unitXmlPath, path, true); }
            catch { }

            return (Sprite)obj;
        }
    }
}
