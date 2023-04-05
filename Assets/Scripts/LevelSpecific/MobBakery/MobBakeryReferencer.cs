using General;
using Generic;
using Generic.Interaction;
using Generic.ShowSystem;
using Injector;
using InputRelated;
using LevelSpecific.MobBakery.Machines;
using LevelSpecific.MobBakery.Npc;
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
            _injectionList.AddType(typeof(BaseMachine));
            _injectionList.AddType(typeof(UISelectionSystem));
            _injectionList.AddType(typeof(BakeryNpc));
            _injectionList.AddType(typeof(PoisonInjector));
            _injectionList.AddType(typeof(IngredientHolder));
            
            _refTable.CreateReference<IItemHolder<InteractionListener>,PlayerTriggerManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<IInputControlProvider>,InputManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IInputManager,InputManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<InputManager,InputManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<BakeryNpc>,BakeryNpcManager>(ReferenceType.FromTransform);
        }
    }
}