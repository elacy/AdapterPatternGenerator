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
        private readonly string _baseNamespace;
        public Type Type { get; private set; }
        public string Name { get { return Declaration.Name; } }
        public CodeTypeDeclaration Declaration { get; set; }
        public bool IsInterface { get { return Declaration.IsInterface; }}
        public bool IsStatic { get; private set; }

        public TypeDeclarationHandler(Type type,string baseNamespace, CodeTypeDeclaration codeTypeDeclaration, bool isStatic)
        {
            _baseNamespace = baseNamespace;
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
            var fields = Type.GetFields(staticFlag | BindingFlags.Public);
            foreach (var field in fields)
            {
                AddField(Declaration, field);
            }
        }

        private void AddField(CodeTypeDeclaration codeTypeDeclaration, FieldInfo fieldInfo)
        {
            var property = new CodeMemberProperty
            {
                Name = fieldInfo.Name,
                Type = new CodeTypeReference(fieldInfo.FieldType),
                HasSet = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral ,
                HasGet = true,
            };
            codeTypeDeclaration.Members.Add(property);
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
