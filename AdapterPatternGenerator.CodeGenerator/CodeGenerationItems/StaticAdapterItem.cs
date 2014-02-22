using System;
using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class StaticAdapterItem : AdapterItem
    {
        private static string GetName(Type originalType)
        {
            return string.Format("{0}StaticAdapter", originalType.Name);
        }
        public StaticAdapterItem(Type originalType, string baseNameSpace)
            : base(originalType, GetNamespace(originalType, baseNameSpace, false), GetName(originalType))
        {
        }
         
        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            codeTypeDeclaration.BaseTypes.Add(typeMap.GetStaticInterface(OriginalType));
            AddMembers(codeTypeDeclaration, BindingFlags.Static | BindingFlags.Public, typeMap);
            return codeTypeDeclaration;
        }
    }
}