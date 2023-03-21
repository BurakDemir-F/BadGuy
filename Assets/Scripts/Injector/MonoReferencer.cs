using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using General;
using Generic;
using Generic.Interaction;
using Generic.Items;
using Generic.Providers;
using InputRelated;
using Managers;
using Npc;
using Player;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Injector
{
    public class MonoReferencer : MonoBehaviour
    {
        [SerializeField] private TypeObjectDict _injectableObjects;
        private MonoReferenceTable _refTable;
        private MonoNeedInjectionList _injectionList;

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
            _injectionList.AddType(typeof(BreakableItem));
            _injectionList.AddType(typeof(InteractionListener));
            _injectionList.AddType(typeof(MovementInput));
            _injectionList.AddType(typeof(PlayerInputHandler));
            _refTable.CreateReference<IItemHolder<IInputControlProvider>,InputManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IObjectProvider<AudioSource>,AudioSourceProvider>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<BreakableItem>,NpcManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<InteractionListener>,PlayerTriggerManager>(ReferenceType.FromTransform);
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
                        Debug.Log($"reference connected : {propType.Name} with {monoObj.name}");
                    }
                }
            }
        }
        
        [System.Serializable]
        public class TypeObjectDict : SerializableDictionary<Type,List<Object>>{}
    }
}