using System;
using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class InstanceAdapterItem : AdapterItem
    {
        private static string GetName(Type originalType)
        {
            return string.Format("{0}Adapter", originalType.Name);
        }
        public InstanceAdapterItem(Type originalType, string baseNameSpace)
            : base(originalType, GetNamespace(originalType, baseNameSpace, false), GetName(originalType))
        {
        }

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMembers(codeTypeDeclaration, BindingFlags.Instance | BindingFlags.Public, typeMap);
            var baseClass = typeMap.BaseInstanceClass.Clone();
            baseClass.TypeArguments.Add(new CodeTypeReference(OriginalType));
            codeTypeDeclaration.BaseTypes.Add(baseClass);
            return codeTypeDeclaration;
        }
    }
}