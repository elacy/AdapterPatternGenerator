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
        private readonly ICodeCompileUnitCreator _codeCompileUnitCreator;
        private readonly ITypeWriter _typeWriter;

        public Generator(ICodeCompileUnitCreator codeCompileUnitCreator, ITypeWriter typeWriter)
        {
            _codeCompileUnitCreator = codeCompileUnitCreator;
            _typeWriter = typeWriter;
        }

        public void GenerateCode(IEnumerable<Type> types, string directoryName)
        {
            var compileUnit = _codeCompileUnitCreator.CreateCodeCompileUnit(types);
            _typeWriter.WriteTypes(compileUnit,directoryName);
        }
    }

    public interface IGenerator
    {
        void GenerateCode(IEnumerable<Type> assembly, string directoryName);
    }
}
