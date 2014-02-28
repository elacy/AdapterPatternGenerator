using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FubuCsProjFile;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ISolutionGenerator
    {
        Solution GenerateProjects(string directoryName, string baseNameSpace, IEnumerable<CodeCompileUnit> codeCompileUnits);
    }
}
