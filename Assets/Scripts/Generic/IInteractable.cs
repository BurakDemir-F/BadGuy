using UnityEngine;

namespace Generic
{
    public interface IInteractable
    {
        void Interact(Collider col);
        void InteractEnd(Collider col);
    }
}