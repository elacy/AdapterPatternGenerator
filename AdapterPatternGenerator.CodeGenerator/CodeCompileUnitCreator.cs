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
            var codeCompileUnits = new List<CodeCompileUnit>{_baseAdapterCreator.CreateBaseClass(baseNameSpace +"."+Constants.ClassesNamespace)};
            var handlers = new List<ITypeDeclarationHandler>();
            foreach (var nameSpace in typesByNamespace)
            {
                foreach (var type in nameSpace)
                {
                    var classTypes =new [] { AddTypeDeclaration(handlers, type,baseNameSpace, true, false),AddTypeDeclaration(handlers, type,baseNameSpace, false, false) };

                    codeCompileUnits.Add(CreateCodeUnit(string.Format("{0}.{1}.{2}", baseNameSpace,Constants.ClassesNamespace,nameSpace.Key), classTypes));
                    var interfaceTypes = new[] { AddTypeDeclaration(handlers, type,baseNameSpace, true, true), AddTypeDeclaration(handlers, type,baseNameSpace, false, true) };
                    codeCompileUnits.Add(CreateCodeUnit(string.Format("{0}.{1}.{2}", baseNameSpace, Constants.InterfacesNamespace, nameSpace.Key), interfaceTypes));
                }
            }
            foreach (var handler in handlers)
            {
                handler.AddMembers(_typeMap);
            }
            return codeCompileUnits;
        }

        private CodeTypeDeclaration AddTypeDeclaration(List<ITypeDeclarationHandler> handlers, Type type, string baseNamespace, bool isStatic, bool isInterface)
        {
            var handler = _typeDeclarationHandler.Create(type, baseNamespace, isInterface, isStatic);
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
