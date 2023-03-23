using General;
using Generic;
using Generic.Interaction;
using Injector;
using InputRelated;
using Managers;
using Player;

namespace LevelSpecific.MobBakery
{
    public class MobBakeryReferencer : MonoReferencer
    {
        protected override void AddReferences()
        {
            _injectionList.AddType(typeof(InteractionListener));
            _injectionList.AddType(typeof(PlayerInputHandler));
            _injectionList.AddType(typeof(MovementInput));
            
            _refTable.CreateReference<IItemHolder<InteractionListener>,PlayerTriggerManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<IInputControlProvider>,InputManager>(ReferenceType.FromTransform);
        }
    }
}