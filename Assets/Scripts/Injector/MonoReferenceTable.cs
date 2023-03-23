using System;
using UnityEngine;
using Utilities;

namespace Injector
{
    public class MonoReferenceTable : MonoBehaviour
    {
        public TypeReferenceDict References;

        public void Init()
        {
            References = new TypeReferenceDict();
        }

        public void CreateReference<TBase, TMono>(ReferenceType refType) where TMono : MonoBehaviour, TBase
        {
            if (References.ContainsKey(typeof(TBase)))
            {
                return;
            }

            var newRef = new Reference(typeof(TBase), typeof(TMono), refType);
            TMono newObj = null;

            if (refType == ReferenceType.Singleton)
                newObj = new GameObject($"{nameof(TBase)}").AddComponent<TMono>();

            else if (refType == ReferenceType.FromTransform)
            {
                newObj = GetComponentInChildren<TMono>();
                
                Debug.Assert(newObj != null, "Transform object is null!");
            }

            if (newObj != null) newObj.transform.SetParent(transform);
            
            newRef.ReferenceObject = newObj;
            References.Add(typeof(TBase), newRef);
            Debug.LogWarning($"### Referencer Reference created: {typeof(TBase)} to {newObj}", newObj.gameObject);
        }


        public TMono GetReference<TBase, TMono>() where TMono : MonoBehaviour, TBase
        {
            if (!References.ContainsKey(typeof(TBase)))
                Debug.Log("I don't have this reference, something wrong here!.");

            var reference = References[typeof(TBase)];
            var refType = reference.Type;

            if (refType == ReferenceType.Singleton || refType == ReferenceType.FromTransform)
                return References[typeof(TBase)].ReferenceObject as TMono;

            if (refType == ReferenceType.New)
            {
                var newObj = new GameObject($"{nameof(TBase)}").AddComponent<TMono>();
                newObj.transform.SetParent(transform);
                return newObj;
            }

            return null;
        }

        public MonoBehaviour GetReference(Type tBase, Type tMono)
        {
            if (!References.ContainsKey(tBase))
                Debug.Log("I don't have this reference, something wrong here!.");
            
            var reference = References[tBase];
            var refType = reference.Type;

            if (refType == ReferenceType.Singleton || refType == ReferenceType.FromTransform)
                return References[tBase].ReferenceObject;

            if (refType == ReferenceType.New)
            {
                var newObj = new GameObject($"{nameof(tBase)}",tMono).GetComponent(tMono);
                newObj.transform.SetParent(transform);
                return newObj as MonoBehaviour;
            }

            return null;
        }
    }

    [System.Serializable]
    public class Reference
    {
        public Type BaseReference;
        public Type ToReference;
        public ReferenceType Type;
        public MonoBehaviour ReferenceObject;

        public Reference(Type baseReference, Type toReference, ReferenceType type)
        {
            BaseReference = baseReference;
            ToReference = toReference;
            Type = type;
        }
    }

    public enum ReferenceType
    {
        None,
        Singleton,
        New,
        FromTransform
    }
    
    public class TypeReferenceDict : SerializableDictionary<Type,Reference>{}
}