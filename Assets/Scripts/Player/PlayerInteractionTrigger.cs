using System;
using Generic;
using Generic.Interaction;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public class PlayerInteractionTrigger : InteractionListener
    {
        private CharacterController _controller;

        public override string Tag => _tag;
        public override Type RequestedComponentType => typeof(IInteractable);
        public override ComponentProvider ComponentProvider => ComponentProvider.Listener;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void OnTriggerEntered(Object other)
        {
            (other as IInteractable).Interact(_controller);
        }

        public override void OnTriggerExited(Object obj)
        {
        }
    }
}