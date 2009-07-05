using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameDemo1.Factory
{
    public class CodeGenerator
    {
        private String _textCode;

        public CodeGenerator(){}
        public void Load(String templateFilePath){
            if (!File.Exists(templateFilePath))
                throw new Exception("I'm sorry! Template file's not found.");

            using (StreamReader rd = new StreamReader(templateFilePath)){
                _textCode = rd.ReadToEnd();
                rd.Close();
            }
        }
        public String GetHostNamespace()
        {
            String name = this.GetType().ToString();
            if (name.IndexOf('.') > 0)
                name = name.Substring(0, name.IndexOf('.'));
            return name;
        }
        public void AddAttrDeclaration(Dictionary<String, String> attrList)
        {
            String code = "";
            foreach (KeyValuePair<String, String> item in attrList)
            {
                code = CreateAttrDeclarationCode(item.Key, item.Value);
                code += Environment.NewLine;
                code += "%extAttribute%";
                _textCode = _textCode.Replace("%extAttribute%", code);
            }
        }
        public String Process(String asmNamespace, String className)
        {
            _textCode = _textCode.Replace("%extAttribute%", "");
            _textCode = _textCode.Replace("%className%", className);
            _textCode = _textCode.Replace("%asmNamespace%", asmNamespace);
            return _textCode;
        }

        public static String CreateAttrDeclarationCode(String variableName, String value)
        {
            string code = "%variableName% = %value%;";
            code = code.Replace("%variableName%", variableName);
            code = code.Replace("%value%", value);
            return code;
        }

    }
}
