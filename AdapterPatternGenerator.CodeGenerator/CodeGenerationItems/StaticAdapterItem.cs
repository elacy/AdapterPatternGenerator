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

        const BindingFlags BindingFlags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMethods(codeTypeDeclaration, OriginalType.GetMethods(BindingFlags), typeMap);
            AddProperties(codeTypeDeclaration, OriginalType.GetProperties(BindingFlags), typeMap);
            AddFields(codeTypeDeclaration, OriginalType.GetFields(BindingFlags), typeMap);
            return codeTypeDeclaration;
        }
    }
}