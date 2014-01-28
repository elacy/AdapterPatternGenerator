using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeMap : ITypeMap
    {

        private readonly Dictionary<Type, CodeTypeReference> _interfaces = new Dictionary<Type, CodeTypeReference>();
        private readonly Dictionary<Type, CodeTypeReference> _classes = new Dictionary<Type, CodeTypeReference>();

        public TypeMap()
        {
        }


        public void Add(Type type, CodeTypeReference instanceInterface, CodeTypeReference instanceClass)
        {
            _interfaces.Add(type, instanceInterface);
            _classes.Add(type, instanceClass);
        }



        public CodeTypeReference GetBaseClass(bool isInterface, bool isStatic)
        {
            throw new NotImplementedException();
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
            return GetReference(type, _classes);
        }

        private CodeTypeReference GetReference(Type type, IReadOnlyDictionary<Type, CodeTypeReference> map)
        {
            var genericTypeDef = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            var codeReference = new CodeTypeReference(type);
            if (map.ContainsKey(genericTypeDef))
            {

                codeReference = map[genericTypeDef];
            }
            foreach (var param in type.GenericTypeArguments)
            {
                codeReference.TypeArguments.Add(GetReference(param, map));
            }
            return codeReference;
        }

    }

    public interface ITypeMap
    {
        CodeTypeReference BaseStaticInterface { get; set; }
        CodeTypeReference BaseStaticClass { get; set; }
        CodeTypeReference BaseInstanceClass { get; set; }
        CodeTypeReference BaseInstanceInterface { get; set; }
        CodeTypeReference GetInstanceInterface(Type type);
        CodeTypeReference GetInstanceClass(Type type);
        void Add(Type type, CodeTypeReference instanceInterface, CodeTypeReference instanceClass);
    }
}
