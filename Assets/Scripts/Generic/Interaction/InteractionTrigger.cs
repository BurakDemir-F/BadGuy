using System;
using System.Collections.Generic;
using General;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Generic.Interaction
{
    public class InteractionTrigger : MonoBehaviour, IItemHolder<InteractionListener>
    {
        public event Action<Object> TriggerEntered;
        public event Action<Object> TriggerExited;
        protected Dictionary<string, InteractionListener> _listenerDict;
        private bool isInitialized;

        private void Awake()
        {
            Init();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            DoTriggerLogic(other, true);
        }

        protected void OnTriggerExit(Collider other)
        {
            DoTriggerLogic(other, false);
        }

        private void DoTriggerLogic(Collider other, bool isEntered)
        {
            var interactedTag = other.tag;
            var isTagExist = _listenerDict.ContainsKey(interactedTag);

            if (!isTagExist)
                return;

            var listener = _listenerDict[interactedTag];
            var listenerType = listener.RequestedComponentType;

            Component component = null;
            if (listener.ComponentProvider != ComponentProvider.None)
            {
                var componentProvider = listener.ComponentProvider == ComponentProvider.Listener
                    ? other.gameObject
                    : gameObject;
                
                if (!componentProvider.TryGetComponent(listenerType, out component))
                    return;
            }

            if (isEntered)
            {
                TriggerEntered?.Invoke(component);
                listener.OnTriggerEntered(component);
            }
            else
            {
                TriggerExited?.Invoke(component);
                listener.OnTriggerExited(component);
            }
        }

        private void Init()
        {
            if (isInitialized)
                return;
            _listenerDict = new Dictionary<string, InteractionListener>();
            isInitialized = true;
        }

        public void Add(InteractionListener item)
        {
            Init();
            var itemTag = item.Tag;
            if (!_listenerDict.ContainsKey(itemTag))
            {
                _listenerDict.Add(itemTag, item);
            }
        }

        public void Remove(InteractionListener item)
        {
            Init();
            if (_listenerDict.ContainsKey(item.Tag))
                _listenerDict.Remove(item.Tag);
        }
    }
}