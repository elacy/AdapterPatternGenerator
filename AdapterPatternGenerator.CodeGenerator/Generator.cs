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
        private readonly ICodeWriter _codeWriter;

        public Generator(ICodeCompileUnitCreator codeCompileUnitCreator, ICodeWriter codeWriter)
        {
            _codeCompileUnitCreator = codeCompileUnitCreator;
            _codeWriter = codeWriter;
        }

        public void GenerateCode(IEnumerable<Type> types, string directoryName)
        {
            var compileUnit = _codeCompileUnitCreator.CreateCodeCompileUnit(types);
            _codeWriter.WriteCompileUnits(compileUnit,directoryName);
        }
    }

    public interface IGenerator
    {
        void GenerateCode(IEnumerable<Type> assembly, string directoryName);
    }
}
