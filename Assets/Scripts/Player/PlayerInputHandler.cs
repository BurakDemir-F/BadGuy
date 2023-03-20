using Generic;
using InputRelated;
using UnityEngine;

namespace Player
{
    public class PlayerInputHandler : InputReceiver<GameActionType>
    {
        [SerializeField] private TransformProvider _transformProvider;
        [SerializeField] private PlayerItemHolder _itemHolder;
        private Vector3 ThrowDirection => _transformProvider.Get().forward;
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