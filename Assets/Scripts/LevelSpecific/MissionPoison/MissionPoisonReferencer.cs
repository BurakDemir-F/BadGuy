using General;
using Generic;
using Generic.Interaction;
using Generic.Items;
using Generic.Items.Helpers;
using Generic.Providers;
using Injector;
using InputRelated;
using InteractableArea;
using Managers;
using Npc;
using Player;
using UnityEngine;

namespace LevelSpecific.MissionPoison
{
    public class MissionPoisonReferencer : MonoReferencer
    {
        protected override void AddReferences()
        {
            _injectionList.AddType(typeof(BreakableItem));
            _injectionList.AddType(typeof(InteractionListener));
            _injectionList.AddType(typeof(MovementInput));
            _injectionList.AddType(typeof(PlayerInputHandler));
            _injectionList.AddType(typeof(HitEffect));
            _injectionList.AddType(typeof(LevelManager));
            
            _refTable.CreateReference<WinBox,WinBox>(ReferenceType.FromTransform);
            _refTable.CreateReference<NpcManager,NpcManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<IInputControlProvider>,InputManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IObjectProvider<AudioSource>,AudioSourceProvider>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<BreakableItem>,NpcManager>(ReferenceType.FromTransform);
            _refTable.CreateReference<IItemHolder<InteractionListener>,PlayerTriggerManager>(ReferenceType.FromTransform);
        }
    }
}