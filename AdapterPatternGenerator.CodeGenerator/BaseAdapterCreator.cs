using System.CodeDom;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class BaseAdapterCreator:IBaseAdapterCreator
    {
        public CodeCompileUnit CreateBaseClass(string namespaceName)
        {
            var unit =  new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(namespaceName);
            unit.Namespaces.Add(codeNamespace);

            var baseType = new CodeTypeDeclaration(Constants.BaseInstanceAdapterName)
            {
                TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public
            };
            codeNamespace.Types.Add(baseType);

            var codeTypeParam = new CodeTypeParameter("T");
            baseType.TypeParameters.Add(codeTypeParam);

            var field = new CodeMemberField(codeTypeParam.Name, Constants.InternalAdapterFieldName)
            {
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            baseType.Members.Add(field);

            var constructor = new CodeConstructor{Attributes = MemberAttributes.Family};
            baseType.Members.Add(constructor);

            var constructorParam = new CodeParameterDeclarationExpression(codeTypeParam.Name, "adapterInstance");
            constructor.Parameters.Add(constructorParam);

            var fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
            var variableReference = new CodeVariableReferenceExpression(constructorParam.Name);
            var statement = new CodeAssignStatement(fieldReference, variableReference);
            constructor.Statements.Add(statement);

            return unit;
        }
    }

    public interface IBaseAdapterCreator
    {
        CodeCompileUnit CreateBaseClass(string nameSpace);
    }
}
