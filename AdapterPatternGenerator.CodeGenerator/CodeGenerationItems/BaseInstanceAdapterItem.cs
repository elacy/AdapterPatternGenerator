using System.CodeDom;
using System.ComponentModel;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class BaseInstanceAdapterItem : CodeGenerationItem
    {
        private static string GetNamespace(string baseNameSpace)
        {
            return CreateNameSpace(baseNameSpace, Constants.ClassesNamespace);
        }
        public BaseInstanceAdapterItem(string baseNameSpace)
            : base(GetNamespace(baseNameSpace), Constants.BaseInstanceAdapterName)
        {

        }

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration =  base.Generate(typeMap);
            codeTypeDeclaration.IsPartial = true;
            codeTypeDeclaration.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;

            var codeTypeParam = new CodeTypeParameter("T");
            codeTypeDeclaration.TypeParameters.Add(codeTypeParam);

            var field = new CodeMemberField(codeTypeParam.Name, Constants.InternalAdapterFieldName)
            {
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            codeTypeDeclaration.Members.Add(field);

            var constructor = new CodeConstructor { Attributes = MemberAttributes.Family };
            codeTypeDeclaration.Members.Add(constructor);

            var constructorParam = new CodeParameterDeclarationExpression(codeTypeParam.Name, "adapterInstance");
            constructor.Parameters.Add(constructorParam);

            var fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
            var variableReference = new CodeVariableReferenceExpression(constructorParam.Name);
            var statement = new CodeAssignStatement(fieldReference, variableReference);
            constructor.Statements.Add(statement);
            return codeTypeDeclaration;
        }
    }
}