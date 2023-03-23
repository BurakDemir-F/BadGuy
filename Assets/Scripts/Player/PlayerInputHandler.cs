using System;
using System.Collections;
using General;
using Generic;
using Injector;
using InputRelated;
using UnityEngine;

namespace Player
{
    public class PlayerInputHandler : InputReceiver<GameActionType>,IInputControlProvider
    {
        [SerializeField] private TransformProvider _transformProvider;
        [SerializeField] private PlayerItemHolder _itemHolder;
        private Vector3 ThrowDirection => _transformProvider.Get().forward;
        public InputType InputType => InputType.PlayerInteraction;
        [InjectReference]
        public IItemHolder<IInputControlProvider> _inputHolder { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(AddHolderCor());
        }

        private IEnumerator AddHolderCor()
        {
            yield return new WaitForSeconds(.1f);
            _inputHolder.Add(this);
        }
        

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputHolder.Remove(this);
        }

        protected override void OnGameActionPerformed(GameActionType gameActionType)
        {
            if(gameActionType != GameActionType.ThrowObject)
                return;
            
            if(!_itemHolder.IsHoldingItem)
                return;

            var item = _itemHolder.item;
            _itemHolder.ReleaseItem();
            var body = item.Rigidbody;
            body.AddForce((ThrowDirection.normalized + Vector3.up * .2f) * item.Data.ThrowForce);
        }

    }

    public enum GameActionType
    {
        None,
        ThrowObject
    }
}