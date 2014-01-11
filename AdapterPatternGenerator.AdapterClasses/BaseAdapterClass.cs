using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.AdapterClasses
{
    public abstract class BaseStaticAdapterClass : BaseAdapterClass
    {

    }

    public abstract class BaseInstanceAdapterClass<T> : BaseAdapterClass, IDisposable
    {
        internal readonly T AdaptedClass;
        protected BaseInstanceAdapterClass(T adaptedClass)
        {
            AdaptedClass = adaptedClass;
        }

        public void Dispose()
        {
            var obj = AdaptedClass as IDisposable;
            if (obj != null)
            {
                obj.Dispose();
            }
        }
    }

    public abstract class BaseAdapterClass
    {
        protected static TInstance Convert<TInstance>(object iAdapter) where TInstance:class
        {
            var adapter = iAdapter as BaseInstanceAdapterClass<TInstance>;
            if (adapter != null)
            {
                return adapter.AdaptedClass;
            }
            return null;
        }
    }
}
