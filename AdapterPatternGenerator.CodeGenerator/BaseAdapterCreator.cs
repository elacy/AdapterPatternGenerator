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

            var baseType = new CodeTypeDeclaration(Constants.BaseInstanceAdapterName);
            codeNamespace.Types.Add(baseType);

            var codeTypeParam = new CodeTypeParameter("T");
            baseType.TypeParameters.Add(codeTypeParam);

            var constructor = new CodeConstructor();
            baseType.Members.Add(constructor);

            var constructorParam = new CodeParameterDeclarationExpression(codeTypeParam.Name, "adapterInstance");
            constructor.Parameters.Add(constructorParam);
            
            return unit;
        }
        private CodeCompileUnit CreateBaseClass2(string baseNameSpace)
        {
            var codeCompileUnit = new CodeCompileUnit();
            var nameSpace = new CodeNamespace(baseNameSpace);
            codeCompileUnit.Namespaces.Add(nameSpace);
            var codeTypeDeclaration = new CodeTypeDeclaration(Constants.BaseInstanceAdapterName);
            nameSpace.Types.Add(codeTypeDeclaration);
            var codeTypeParam = new CodeTypeParameter("T");
            codeTypeDeclaration.TypeParameters.Add(codeTypeParam);
            codeTypeDeclaration.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
            var field = new CodeMemberField(codeTypeParam.Name, Constants.InternalAdapterFieldName)
            {
                Attributes = MemberAttributes.FamilyAndAssembly
            };
            codeTypeDeclaration.Members.Add(field);

            var constructor = new CodeConstructor { Attributes = MemberAttributes.Family };
            var constructorParam = new CodeParameterDeclarationExpression(codeTypeParam.Name, "adaptedInstance");
            constructor.Parameters.Add(constructorParam);
            var fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
            constructor.Statements.Add(new CodeAssignStatement(fieldReference, new CodeVariableReferenceExpression(constructorParam.Name)));
            codeTypeDeclaration.Members.Add(constructor);
            return codeCompileUnit;
        }
    }

    public interface IBaseAdapterCreator
    {
        CodeCompileUnit CreateBaseClass(string nameSpace);
    }
}
