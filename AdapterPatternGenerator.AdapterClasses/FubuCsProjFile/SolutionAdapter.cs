using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile;
using FubuCsProjFile;

namespace AdapterPatternGenerator.AdapterClasses.FubuCsProjFile
{
    public class SolutionAdapter:BaseInstanceAdapterClass<Solution>, ISolutionAdapter
    {
        public SolutionAdapter(Solution adaptedClass) : base(adaptedClass)
        {
        }

        public string Version
        {
            get { return AdaptedClass.Version; }
            set { AdaptedClass.Version = value; }
        }

        public ISolutionProjectAdapter AddProject(string projectName)
        {
            return new SolutionProjectAdapter(AdaptedClass.AddProject(projectName));
        }
    }
}
