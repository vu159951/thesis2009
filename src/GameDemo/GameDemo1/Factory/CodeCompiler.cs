using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace GameDemo1.Factory
{
    public class CodeCompiler
    {
        private CompilerParameters parameters;
        public CodeCompiler()
        {
            parameters = new CompilerParameters();
        }
        public void AddReference(String value)
        {
            parameters.ReferencedAssemblies.Add(value);
        }
        public CompilerResults Compile(string source)
        {
            parameters = new CompilerParameters();

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = false;
            parameters.TempFiles = new TempFileCollection(".", false);

            CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");
            return compiler.CompileAssemblyFromSource(parameters, source);
        }
        public CompilerResults Compile(string source, string outputFile)
        {
            parameters.GenerateExecutable = false;               // Generate a class library instead of an executable.
            parameters.GenerateInMemory = true;                  // Save the assembly as a physical file.
            parameters.IncludeDebugInformation = false;          // Generate debug information.
            parameters.OutputAssembly = outputFile;              // Set the assembly file name to generate.
            //parms.CompilerOptions = "/optimize";            // Set compiler argument to optimize output.

            // Set a temporary files collection.
            // The TempFileCollection stores the temporary files
            // generated during a build in the current directory,
            // and does not delete them after compilation.
            parameters.TempFiles = new TempFileCollection(".", false);

            CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");
            return compiler.CompileAssemblyFromSource(parameters, source);
        }
    }
}
