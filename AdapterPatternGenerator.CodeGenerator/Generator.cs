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

        public void GenerateCode(List<Type> types, string directoryName, string baseNameSpace)
        {
            var compileUnit = _codeCompileUnitCreator.CreateCodeCompileUnit(types,baseNameSpace);
            _codeWriter.WriteCompileUnits(compileUnit,directoryName);
        }
    }

    public interface IGenerator
    {
        void GenerateCode(List<Type> assembly, string directoryName, string baseNameSpace);
    }
}
