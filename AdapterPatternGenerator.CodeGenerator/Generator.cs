using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class Generator:IGenerator
    {
        private readonly ITypeDeclarationCreator _typeDeclarationCreator;
        private readonly ITypeWriter _typeWriter;

        public Generator(ITypeDeclarationCreator typeDeclarationCreator, ITypeWriter typeWriter)
        {
            _typeDeclarationCreator = typeDeclarationCreator;
            _typeWriter = typeWriter;
        }

        public void GenerateCode(IEnumerable<Type> types, string directoryName)
        {
            var typeDeclarations = _typeDeclarationCreator.CreateTypes(types);
            _typeWriter.WriteTypes(typeDeclarations, directoryName);
        }
    }

    public interface IGenerator
    {
        void GenerateCode(IEnumerable<Type> assembly, string directoryName);
    }
}
