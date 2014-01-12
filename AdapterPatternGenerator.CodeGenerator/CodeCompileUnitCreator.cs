using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class CodeCompileUnitCreator : ICodeCompileUnitCreator
    {
        private readonly ITypeDeclarationHandlerFactory _typeDeclarationHandler;
        private readonly ITypeMap _typeMap;
        private readonly IBaseAdapterCreator _baseAdapterCreator;

        public CodeCompileUnitCreator(ITypeDeclarationHandlerFactory typeDeclarationHandler, ITypeMap typeMap, IBaseAdapterCreator baseAdapterCreator)
        {
            _typeDeclarationHandler = typeDeclarationHandler;
            _typeMap = typeMap;
            _baseAdapterCreator = baseAdapterCreator;
        }

        public IEnumerable<CodeCompileUnit> CreateCodeCompileUnit(List<Type> types,string baseNameSpace)
        {
            var typesByNamespace = types.GroupBy(x => x.Namespace);
            var codeCompileUnits = new List<CodeCompileUnit>{_baseAdapterCreator.CreateBaseClass(baseNameSpace)};
            var handlers = new List<ITypeDeclarationHandler>();
            foreach (var nameSpace in typesByNamespace)
            {
                foreach (var type in nameSpace)
                {
                    var classTypes =new [] { AddTypeDeclaration(handlers, type, true, false),AddTypeDeclaration(handlers, type, false, false) };
                    codeCompileUnits.Add(CreateCodeUnit(string.Format("{0}.Classes.{1}", baseNameSpace,nameSpace.Key), classTypes));
                    var interfaceTypes = new[] { AddTypeDeclaration(handlers, type, true, true), AddTypeDeclaration(handlers, type, false, true) };
                    codeCompileUnits.Add(CreateCodeUnit(string.Format("{0}.Interfaces.{1}", baseNameSpace, nameSpace.Key), interfaceTypes));
                }
            }
            foreach (var handler in handlers)
            {
                handler.AddMembers(_typeMap);
            }
            return codeCompileUnits;
        }

        private CodeTypeDeclaration AddTypeDeclaration(List<ITypeDeclarationHandler> handlers, Type type, bool isStatic, bool isInterface)
        {
            var handler = _typeDeclarationHandler.Create(type, isInterface, isStatic);
            handlers.Add(handler);
            _typeMap.Add(type, handler.Declaration, isInterface && !isStatic);
            return handler.Declaration;
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
