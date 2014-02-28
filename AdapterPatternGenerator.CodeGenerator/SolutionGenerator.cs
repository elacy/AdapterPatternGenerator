using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile;
using FubuCsProjFile;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class SolutionGenerator : ISolutionGenerator
    {
        private readonly ICodeWriter _codeWriter;

        public SolutionGenerator(ICodeWriter codeWriter)
        {
            _codeWriter = codeWriter;
        }

        public Solution GenerateProjects(string directoryName, string baseNameSpace, IEnumerable<CodeCompileUnit> codeCompileUnits)
        {
            var path = Path.Combine(directoryName, baseNameSpace);
            var solution = Solution.CreateNew(path, baseNameSpace);
            var classesProject = solution.AddProject(Constants.ClassesNamespace);
            
            var interfacesProject = solution.AddProject(Constants.InterfacesNamespace);

            foreach (var codeCompileUnit in codeCompileUnits)
            {
                if (codeCompileUnit.Namespaces[0].Types[0].IsInterface)
                {
                    var interfacePath = GetPath(codeCompileUnit, baseNameSpace + "." + Constants.InterfacesNamespace);
                    interfacesProject.Project.Add(new CodeFile(interfacePath));
                }
                else
                {
                    var classPath = GetPath(codeCompileUnit, baseNameSpace + "." + Constants.ClassesNamespace);
                    classesProject.Project.Add(new CodeFile(classPath));
                }
            }

            return solution;
        }

        private static string GetPath(CodeCompileUnit codeCompileUnit, string baseNameSpace)
        {
            var nameSpace = codeCompileUnit.Namespaces[0];
            var type = nameSpace.Types[0];
            var fullName = nameSpace.Name + "." + type.Name;
            fullName = fullName.Substring(baseNameSpace.Length + 1);
            return Path.Combine(fullName.Split('.')) + ".cs";
        }
    }
}
