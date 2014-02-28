using FubuCsProjFile;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ISolutionWriter
    {
        void WriteSolution(Solution solution);
    }
}