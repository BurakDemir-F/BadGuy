using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Injector
{
    public class MonoReferencer : MonoBehaviour
    {
        [SerializeField] private TypeObjectDict _injectableObjects;
        protected MonoReferenceTable _refTable;
        protected MonoNeedInjectionList _injectionList;

        private void Awake()
        {
            InitializeAndTrackReferences();
            GetComponents();
            ConnectReferences();
        }

        private void InitializeAndTrackReferences()
        {
            Init();
            AddReferences();
            _injectableObjects = new TypeObjectDict();
            foreach (var injectionType in _injectionList)
            {
                var injectableObjects = FindObjectsOfType(injectionType, true);
                _injectableObjects.Add(injectionType,injectableObjects.ToList());
            }
        }

        private void Init()
        {
            GetComponents();
            
            _refTable.Init();
            _injectionList.Init();
        }

        protected virtual void AddReferences()
        {
            
        }

        private void GetComponents()
        {
            _refTable = gameObject.GetComponentEnsure<MonoReferenceTable>();
            _injectionList = gameObject.GetComponentEnsure<MonoNeedInjectionList>();
        }

        private void ConnectReferences()
        {
            foreach (var injectionType in _injectionList)
            {
                var injectableObjects = _injectableObjects[injectionType];
                if(injectableObjects == null || injectableObjects.Count == 0)
                    continue;
                
                foreach (var iObj in injectableObjects)
                {
                    var properties = iObj.GetType().GetProperties();

                    foreach (var propInfo in properties)
                    {
                        var attribute = propInfo.GetCustomAttribute(typeof(InjectReferenceAttribute));
                        if(attribute == null)
                            continue;
                        var propType = propInfo.PropertyType;
                        var monoObj = _refTable.GetReference(propType, injectionType);
                        propInfo.SetValue(iObj,monoObj);
                        Debug.LogWarning(
                            $" ### Referencer,reference connected : type of object : {iObj.GetType()},{propType.Name} with {monoObj.name}");
                    }
                }
            }
        }
        
        [System.Serializable]
        public class TypeObjectDict : SerializableDictionary<Type,List<Object>>{}
    }
}