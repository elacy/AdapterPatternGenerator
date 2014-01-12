using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeDeclarationHandler:ITypeDeclarationHandler
    {
        public Type Type { get; private set; }
        public string Name { get { return Declaration.Name; } }
        public CodeTypeDeclaration Declaration { get; set; }
        public bool IsInterface { get { return Declaration.IsInterface; }}
        public bool IsStatic { get; private set; }

        public TypeDeclarationHandler(Type type, CodeTypeDeclaration codeTypeDeclaration, bool isStatic)
        {
            Type = type;
            Declaration = codeTypeDeclaration;
            IsStatic = isStatic;
        }

        public void AddMembers( ITypeMap typeMap)
        {
            var staticFlag = IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            var properties = Type.GetProperties(staticFlag | BindingFlags.Public);
            foreach (var property in properties)
            {
                AddProperty(Declaration, property);
            }
        }

        private void AddProperty(CodeTypeDeclaration codeTypeDeclaration, PropertyInfo propertyInfo)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyInfo.Name,
                Type = new CodeTypeReference(propertyInfo.PropertyType),
                HasSet = propertyInfo.CanWrite,
                HasGet = propertyInfo.CanRead,
                
                
            };
            codeTypeDeclaration.Members.Add(property);
        }
    }
}
