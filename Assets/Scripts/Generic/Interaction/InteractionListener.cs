using System;
using General;
using Injector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Generic.Interaction
{
    public abstract class InteractionListener : MonoBehaviour
    {
        [InjectReference]
        public IItemHolder<InteractionListener> _itemHolder { get; set; }
        
        [SerializeField]protected string _tag;
        
        public abstract string Tag { get;}
        public abstract Type RequestedComponentType { get;}
        public abstract ComponentProvider ComponentProvider { get; }

        public abstract void OnTriggerEntered(Object obj);
        public abstract void OnTriggerExited(Object obj);

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }

        protected void StartListening()
        {
            _itemHolder.Add(this);
        }

        protected void StopListening()
        {
            _itemHolder.Remove(this);
        }
    }
}