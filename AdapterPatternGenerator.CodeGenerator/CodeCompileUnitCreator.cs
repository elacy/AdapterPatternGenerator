using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class CodeCompileUnitCreator : ICodeCompileUnitCreator
    {
        private readonly ITypeDeclarationCreator _typeDeclarationCreator;

        public CodeCompileUnitCreator(ITypeDeclarationCreator typeDeclarationCreator)
        {
            _typeDeclarationCreator = typeDeclarationCreator;
        }

        public IEnumerable<CodeCompileUnit> CreateCodeCompileUnit(IEnumerable<Type> types)
        {
            var typesByNamespace = types.GroupBy(x => x.Namespace);
            foreach (var nameSpace in typesByNamespace)
            {
                foreach (var type in nameSpace)
                {
                    var typeDeclarations = _typeDeclarationCreator.CreateTypes(type).ToList();
                    var classes = typeDeclarations.Where(x => x.IsClass);
                    yield return CreateCodeUnit(string.Format("Classes.{0}", nameSpace.Key), classes);
                    var interfaces = typeDeclarations.Where(x => x.IsInterface);
                    yield return CreateCodeUnit(string.Format("Interfaces.{0}", nameSpace.Key), interfaces);
                }
            }
        }

        private CodeCompileUnit CreateCodeUnit(string namespaceName,
            IEnumerable<CodeTypeDeclaration> codeTypeDeclarations)
        {
            var nameSpace = new CodeNamespace(namespaceName);
            foreach (var codeTypeDeclaration in codeTypeDeclarations)
            {
                nameSpace.Types.Add(codeTypeDeclaration);
            }
            var codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.Namespaces.Add(nameSpace);
            return codeCompileUnit;
        }

    }
}
