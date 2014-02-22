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
        private readonly ISolutionGenerator _solutionGenerator;
        private readonly ISolutionWriter _solutionWriter;

        public Generator(ICodeCompileUnitCreator codeCompileUnitCreator, ICodeWriter codeWriter, ISolutionGenerator solutionGenerator,ISolutionWriter solutionWriter)
        {
            _codeCompileUnitCreator = codeCompileUnitCreator;
            _codeWriter = codeWriter;
            _solutionGenerator = solutionGenerator;
            _solutionWriter = solutionWriter;
        }

        public void GenerateCode(List<Type> types, string directoryName, string baseNameSpace)
        {
            var compileUnit = _codeCompileUnitCreator.CreateCodeCompileUnit(types,baseNameSpace).ToList();
            _codeWriter.WriteCompileUnits(compileUnit,directoryName);
            var solution = _solutionGenerator.GenerateProjects(directoryName,baseNameSpace,compileUnit);
            _solutionWriter.WriteSolution(solution);

        }
    }

    public interface IGenerator
    {
        void GenerateCode(List<Type> assembly, string directoryName, string baseNameSpace);
    }
}
