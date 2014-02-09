using System;
using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class InterfaceItem:CopyCodeGenerationItem
    {
        public InterfaceItem(Type originalType, string baseNameSpace) : base(originalType, baseNameSpace)
        {

        }

        const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            AddMembers(codeTypeDeclaration, BindingFlags.Instance | BindingFlags.Public,typeMap);
            return codeTypeDeclaration;
        }
    }
}