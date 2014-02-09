using System;
using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class InstanceInterfaceAdapterItem : AdapterItem
    {
        private static string GetName(Type originalType)
        {
            return string.Format("I{0}Adapter", originalType.Name);
        }
        public InstanceInterfaceAdapterItem(Type originalType, string baseNameSpace)
            : base(originalType, GetNamespace(originalType, baseNameSpace, true), GetName(originalType))
        {

        }

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMembers(codeTypeDeclaration, BindingFlags.Instance | BindingFlags.Public, typeMap);    
            codeTypeDeclaration.IsInterface = true;
            return codeTypeDeclaration;
        }
    }
}