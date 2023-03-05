using System;
using TMPro;
using UnityEngine;

namespace Generic
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerArea : MonoBehaviour
    {
        private string _tag;
        private Collider _triggerCollider;
        
        public event Action<TriggerArea> TriggerEnter; 
        public event Action<TriggerArea> TriggerExit; 
        public void Setup(string compareTag)
        {
            _tag = compareTag;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag(_tag)) 
                return;

            _triggerCollider = other;
            TriggerEnter?.Invoke(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if(other != _triggerCollider)
                return;

            _triggerCollider = null;
            TriggerExit?.Invoke(this);
        }
    }
}