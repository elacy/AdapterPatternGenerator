using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile;
using FubuCsProjFile;

namespace AdapterPatternGenerator.AdapterClasses.FubuCsProjFile
{
    public class SolutionStaticAdapter:BaseStaticAdapterClass, ISolutionStaticAdapter
    {
        public ISolutionAdapter CreateNew(string directory, string name)
        {
            var solution = Solution.CreateNew(directory, name);
            solution.Save();
            return new SolutionAdapter(solution);
        }
        public string VS2010 { get { return Solution.VS2010; } }
        public string VS2012 { get { return Solution.VS2012; } }
        public string VS2013 { get { return Solution.VS2013; } }
    }
}
