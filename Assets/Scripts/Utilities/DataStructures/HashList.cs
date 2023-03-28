using System.Collections;
using System.Collections.Generic;

namespace Utilities.DataStructures
{
    public class HashList<T> : IEnumerable<T>
    {
        private List<T> _list;
        private HashSet<T> _hashSet;
        public HashList()
        {
            _list = new List<T>();
            _hashSet = new HashSet<T>();
        }

        public T this[int index] => _list[index];
        public int Count => _list.Count;

        public void Add(T item)
        {
            if (_hashSet.Contains(item))
                return;
            
            _list.Add(item);
            _hashSet.Add(item);
        }

        public void Remove(T item)
        {
            if(!_hashSet.Contains(item))
                return;

            _list.Remove(item);
            _hashSet.Remove(item);
        }

        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}