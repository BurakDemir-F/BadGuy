using System;
using Generic.Interaction;
using Generic.Items;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public class PlayerItemHolder : InteractionListener
    {
        [SerializeField] private Transform _holderTransform;
        public MovementBasedRotationItem item { get; private set; }
        public bool IsHoldingItem { get; private set; }
        public override string Tag => _tag;
        public override Type RequestedComponentType => typeof(Collider);
        public override ComponentProvider ComponentProvider => ComponentProvider.Listener;

        public override void OnTriggerEntered(Object obj)
        {
            var other = obj as Collider;
            
            if(IsHoldingItem)
                return;

            IsHoldingItem = true;

            item = other.GetComponent<MovementBasedRotationItem>();
            item.BindTransform(_holderTransform);
        }

        public override void OnTriggerExited(Object obj)
        {
        }

        public void ReleaseItem()
        {
            item.ReleaseTransform();
            IsHoldingItem = false;
            item = null;
        }
    }
}