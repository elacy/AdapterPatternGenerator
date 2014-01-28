using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public static class Constants
    {
        public const string BaseInstanceAdapterName = "BaseInstanceAdapter";
        public const string InternalAdapterFieldName = "AdapterInstance";
        public const string InterfacesNamespace = "AdapterInterfaces";
        public const string ClassesNamespace = "AdapterClasses";
        public const string GeneratedLanguage = "CSharp";
        public const string GeneratedBracingStyle = "C";
        public const string CodeGenerationAttribute = "System.CodeDom.Compiler.GeneratedCodeAttribute";
    }
}
