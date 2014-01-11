using System.IO;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.AdapterClasses.System.IO
{
    public class DirectoryStaticAdapter : BaseStaticAdapterClass, IDirectoryStaticAdapter
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

    }
}
