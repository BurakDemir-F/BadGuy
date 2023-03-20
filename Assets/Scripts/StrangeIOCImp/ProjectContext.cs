using General;
using Generic.Items;
using Npc;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace StrangeIOCImp
{
    public class ProjectContext : MVCSContext
    {
        public ProjectContext (MonoBehaviour view) : base(view)
        {
        }

        public ProjectContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        protected override void mapBindings()
        {
            injectionBinder.Bind<IItemHolder<BreakableItem>>().To<NpcManager>().ToSingleton();
        }
    }
}