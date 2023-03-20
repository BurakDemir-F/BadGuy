using System.Collections.Generic;

namespace General
{
    public interface IItemHolder<in T>
    {
        public void Add(T item);
        public void Remove(T item);
    }
}
