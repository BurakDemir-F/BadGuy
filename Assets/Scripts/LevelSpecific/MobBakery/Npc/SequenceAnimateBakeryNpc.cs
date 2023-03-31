using System;
using Generic.Animation;

namespace LevelSpecific.MobBakery.Npc
{
    public class SequenceAnimateBakeryNpc : SimpleAnimateSequence<BakeryNpcAnimType,BakeryNpcAnimation>
    {
        private void Start()
        {
            PlaySequence();
        }
    }
}