using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [System.Serializable]
    public class SerializableHashSet<T> : HashSet<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<T> _hashList = new();
        public void OnBeforeSerialize()
        {
            _hashList.Clear();
            _hashList.AddRange(this);
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var hashT in _hashList)
            {
                if(Contains(hashT))
                    continue;
                Add(hashT);
            }
        }
    }
}