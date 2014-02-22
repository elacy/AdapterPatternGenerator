using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeMap : ITypeMap
    {

        private readonly Dictionary<string, CodeTypeReference> _interfaces = new Dictionary<string, CodeTypeReference>();
        private readonly Dictionary<string, CodeTypeReference> _staticInterfaces = new Dictionary<string, CodeTypeReference>();
        private readonly Dictionary<string, CodeTypeReference> _classes = new Dictionary<string, CodeTypeReference>();
        
        public void Add(Type type, CodeTypeReference instanceInterface, CodeTypeReference instanceClass)
        {
            var reference = new CodeTypeReference(type);
            _interfaces.Add(reference.BaseType, instanceInterface);
            _classes.Add(reference.BaseType, instanceClass);
        }
        public void Add(Type type, CodeTypeReference staticInterface)
        {
            var reference = new CodeTypeReference(type);
            _staticInterfaces.Add(reference.BaseType, staticInterface);
        }
        public CodeTypeReference BaseStaticInterface { get; set; }
        public CodeTypeReference BaseStaticClass { get; set; }
        public CodeTypeReference BaseInstanceClass { get; set; }
        public CodeTypeReference BaseInstanceInterface { get; set; }

        public CodeTypeReference GetInstanceInterface(Type type)
        {
            return GetReference(type, _interfaces);
        }

        public CodeTypeReference GetInstanceClass(Type type)
        {
            return GetReference(type,_classes);
        }
        public CodeTypeReference GetStaticInterface(Type type)
        {
            return GetReference(type, _staticInterfaces);
        }


        private static CodeTypeReference GetReference(Type type, IReadOnlyDictionary<string, CodeTypeReference> map)
        {
            var codeReference = new CodeTypeReference(type);
            RecurseReplace(codeReference, map);
            return codeReference;
        }

        private static void RecurseReplace(CodeTypeReference codeReference, IReadOnlyDictionary<string, CodeTypeReference> map)
        {
            if (map.ContainsKey(codeReference.BaseType))
            {
                codeReference.BaseType = map[codeReference.BaseType].BaseType;
            }
        }

    }

    public interface ITypeMap
    {
        CodeTypeReference BaseStaticInterface { get; set; }
        CodeTypeReference BaseStaticClass { get; set; }
        CodeTypeReference BaseInstanceClass { get; set; }
        CodeTypeReference BaseInstanceInterface { get; set; }
        CodeTypeReference GetInstanceInterface(Type type);
        CodeTypeReference GetStaticInterface(Type type);
        CodeTypeReference GetInstanceClass(Type type);
        void Add(Type type, CodeTypeReference staticInterface);
        void Add(Type type, CodeTypeReference instanceInterface, CodeTypeReference instanceClass);
    }
}
