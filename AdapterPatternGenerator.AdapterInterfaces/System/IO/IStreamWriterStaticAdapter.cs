using System;

namespace AdapterPatternGenerator.AdapterInterfaces.System.IO
{
    public interface IStreamWriterStaticAdapter
    {
        IStreamWriterAdapter NewUp(string path);
    }
}