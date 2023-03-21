using UnityEngine;

namespace Generic.Items
{
    public class ItemIndicator : FlyObjectBehaviour
    {
        [SerializeField] protected bool _enabled;
        private void OnTriggerEnter(Collider other)
        {
            if(!_enabled)
                return;
            
            if (other.CompareTag("Player"))
            {
                EnableIndicator();
                Debug.Log("indicator enable called");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_enabled)
                return;
            
            if (other.CompareTag("Player"))
            {
                DisableIndicator();
                Debug.Log("indicator enable called");
            }
        }
    }
}