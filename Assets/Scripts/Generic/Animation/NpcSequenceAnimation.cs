using System;
using Npc;

namespace Generic.Animation
{
    public class NpcSequenceAnimation : SimpleAnimateSequence<NpcAnimType,NpcAnimation>
    {
        private void Start()
        {
            PlaySequence();
        }
    }
}