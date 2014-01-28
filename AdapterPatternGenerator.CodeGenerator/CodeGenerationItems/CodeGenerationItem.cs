using System.CodeDom;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public abstract class CodeGenerationItem
    {
        protected CodeGenerationItem(string nameSpace, string name)
        {
            NameSpace = nameSpace;
            Name = name;
            CodeTypeReference =  new CodeTypeReference(CreateNameSpace(NameSpace,Name));
        }

        public virtual CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            return new CodeTypeDeclaration(Name)
            {
                CustomAttributes =
                    new CodeAttributeDeclarationCollection
                    {
                        new CodeAttributeDeclaration(Constants.CodeGenerationAttribute)
                    }
            };
        }

        public string NameSpace { get; private set; }
        public string Name { get; private set; }

        public CodeTypeReference CodeTypeReference { get; private set; }

        protected static string GetNamespace(string baseNameSpace, bool isInterface)
        {
            var nameSpace = isInterface ? Constants.InterfacesNamespace : Constants.ClassesNamespace;
            return baseNameSpace + "." + nameSpace;
        }
        protected static string CreateNameSpace(params string[] parts)
        {
            return string.Join(".", parts);
        }
    }
}
