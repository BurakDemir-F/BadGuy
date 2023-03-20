using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Injector
{
    public class MonoNeedInjectionList : MonoBehaviour, IEnumerable<Type>
    {
        private HashSet<Type> _injectionNeedyTypes;
        public void Init()
        {
            _injectionNeedyTypes = new HashSet<Type>();
        }
        public void AddType(Type newType)
        {
            if(_injectionNeedyTypes.Contains(newType))
                return;

            _injectionNeedyTypes.Add(newType);
        }

        public void AddTypes(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (_injectionNeedyTypes.Contains(type))
                    continue;

                _injectionNeedyTypes.Add(type);
            }
        }

        public IEnumerator<Type> GetEnumerator()
        {
            foreach (var type in _injectionNeedyTypes)
            {
                yield return type;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}