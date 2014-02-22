using System;
using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class StaticInterfaceAdapterItem : AdapterItem
    {
        internal static string GetName(Type originalType)
        {
            return string.Format("I{0}StaticAdapter", originalType.Name);
        }
        public StaticInterfaceAdapterItem(Type originalType, string baseNameSpace)
            : base(originalType, GetNamespace(originalType, baseNameSpace, true), GetName(originalType))
        {
        }
        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMembers(codeTypeDeclaration, BindingFlags.Static | BindingFlags.Public, typeMap);    
            codeTypeDeclaration.IsInterface = true;
            return codeTypeDeclaration;
        }
    }
}