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
using System.Runtime.Serialization.Formatters.Binary;

namespace GameDemo1.Factory
{
    public abstract class SpriteManager : Dictionary<string, Sprite>
    {
        protected Game _game;
        protected SpriteDataReader reader;
        protected String ASM_EXTENSION = ".dll";
        protected String SPEC_EXTENSION = ".xml";
        protected CodeCompiler compiler;
        protected CodeGenerator codeGen;
        protected String NS;
        protected Dictionary<String, String> attrList;

        public SpriteManager(Game game){
            _game = game;
            codeGen = new CodeGenerator();
            compiler = new CodeCompiler();
            attrList = new Dictionary<string, string>();

            NS = codeGen.GetHostNamespace() + ".Objects";            
            compiler.AddReference("System.dll");
            compiler.AddReference("System.Data.dll");
            compiler.AddReference(GlobalDTO.XNA_FRAMEWORK_PATH + "Microsoft.Xna.Framework.dll");
            compiler.AddReference(GlobalDTO.XNA_FRAMEWORK_PATH + "Microsoft.Xna.Framework.Game.dll");
            compiler.AddReference("GameSharedObject.dll");
        }
        public void Load(String ObjSpritePath, String SpecSpritePath)
        {
            string[] pluginFiles = Directory.GetFiles(ObjSpritePath, "*" + this.ASM_EXTENSION);

            for (int i = 0; i < pluginFiles.Length; i++){
                string spriteName = Path.GetFileNameWithoutExtension(pluginFiles[i]);
                this.Load(spriteName, ObjSpritePath, SpecSpritePath);
            }
        }
        
        public virtual Sprite Add(String unitXmlPath, String ObjSpritePath, String SpecSpritePath){
            // GENERATE code
            String spriteName = reader.Load(unitXmlPath).Name;
            bool isAbleCompile = true;

            if (attrList.Count > 0)
                codeGen.AddAttrDeclaration(attrList);       // Add extensible properties to constructor of the dynamic class

            String code = codeGen.Process(codeGen.GetHostNamespace(), spriteName);

            // BUILD & COMPILE code - Run the compiler and build the assembly
            String dllFile = ObjSpritePath+ spriteName + this.ASM_EXTENSION;

            if (File.Exists(dllFile)) { try { File.Delete(dllFile); } catch { isAbleCompile = false; } }         // update to new obj
            if (isAbleCompile){
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
            }
            // Copy specification file to the owner folder
            String specFile = SpecSpritePath + Path.GetFileName(unitXmlPath);
            try { File.Copy(unitXmlPath, specFile, true); }
            catch { }

            return this.Load(spriteName, ObjSpritePath, SpecSpritePath);
        }
        /// <summary>
        /// Hàm chính thực hiện việc load
        /// </summary>
        /// <param name="spriteName">Tên của đối tượng cần load</param>
        /// <param name="ObjSpritePath">Đường dẫn chứa tập tin có mã IL chứa tập tin cần load</param>
        /// <param name="SpecSpritePath">Đường dẫn chưa tập tin đặc tả</param>
        /// <returns>Đối tượng được load lên</returns>
        public virtual Sprite Load(String spriteName, String ObjSpritePath, String SpecSpritePath)
        {
            String dllFile = ObjSpritePath + spriteName + this.ASM_EXTENSION;
            String specFile = SpecSpritePath + spriteName + this.SPEC_EXTENSION;

            // Check existing object
            if (this.ContainsKey(spriteName))
                return this[spriteName];

            // LOAD object - Load the generated assembly into the ApplicationDomain 
            Assembly asm = Assembly.LoadFrom(dllFile);
            Type t = asm.GetType(NS + "." + spriteName);
            // BindingFlags enumeration specifies flags that control binding and 
            // the way in which the search for members and types is conducted by reflection. 
            // The following specifies the Access Control of the bound type
            BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance;
            // Construct an instance of the type and invoke the member method
            Object obj = t.InvokeMember(spriteName, bflags |
                BindingFlags.CreateInstance, null, null,
                new object[]{
                    _game, 
                    specFile}
                );
            Sprite sprite = (Sprite)obj;
            this.Add(spriteName, sprite);
            return sprite;
        }
    }
}
