using System;
using System.Collections;
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

        private  void OnEnable()
        {
            StartCoroutine(StartListeningCor());
        }

        private void OnDisable()
        {
            StopListening();
        }

        private IEnumerator StartListeningCor()
        {
            yield return new WaitForSeconds(.1f);
            StartListening();
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