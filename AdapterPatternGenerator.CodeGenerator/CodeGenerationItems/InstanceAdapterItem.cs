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

        const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMethods(codeTypeDeclaration, OriginalType.GetMethods(BindingFlags), typeMap);
            AddProperties(codeTypeDeclaration, OriginalType.GetProperties(BindingFlags), typeMap);
            AddFields(codeTypeDeclaration, OriginalType.GetFields(BindingFlags), typeMap);
            var baseClass = typeMap.BaseInstanceClass.Clone();
            baseClass.TypeArguments.Add(new CodeTypeReference(OriginalType));
            codeTypeDeclaration.BaseTypes.Add(baseClass);
            return codeTypeDeclaration;
        }
    }
}