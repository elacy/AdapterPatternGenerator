using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.CodeGenerator.CodeGenerationItems;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class CodeCompileUnitCreator : ICodeCompileUnitCreator
    {
        private readonly ITypeMap _typeMap;

        public CodeCompileUnitCreator(ITypeMap typeMap)
        {
            _typeMap = typeMap;

        }

        public IEnumerable<CodeCompileUnit> CreateCodeCompileUnit(List<Type> types, string baseNameSpace)
        {
            var items = new List<CodeGenerationItem>();
            var baseInstanceClass = new BaseInstanceAdapterItem(baseNameSpace);
            _typeMap.BaseInstanceClass = baseInstanceClass.CodeTypeReference;
            items.Add(baseInstanceClass);
            foreach (var type in types)
            {
                if (type.IsEnum)
                {
                    items.Add(new EnumItem(baseNameSpace, type));
                }
                else if (type.IsInterface)
                {
                    items.Add(new InterfaceItem(type, baseNameSpace));
                }
                else
                {
                    if (!type.IsSealed || !type.IsAbstract)
                    {
                        var instanceInterface = new InstanceInterfaceAdapterItem(type, baseNameSpace);
                        items.Add(instanceInterface);
                        var instanceClass = new InstanceAdapterItem(type, baseNameSpace);
                        items.Add(instanceClass);
                        _typeMap.Add(type,instanceInterface.CodeTypeReference,instanceClass.CodeTypeReference);
                    }
                    items.Add(new StaticAdapterItem(type, baseNameSpace));
                    var staticInterface = new StaticInterfaceAdapterItem(type, baseNameSpace);
                    _typeMap.Add(type, staticInterface.CodeTypeReference);
                    items.Add(staticInterface);
                }
            }
            foreach (var item in items)
            {
                yield return new CodeCompileUnit
                {
                    Namespaces = {new CodeNamespace(item.NameSpace)
                {
                    Types = {item.Generate(_typeMap)}
                }
                    }
                };
            }
        }


    }
}
