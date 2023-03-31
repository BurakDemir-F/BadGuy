using System;
using Generic.Animation;
using UnityEngine;

namespace LevelSpecific.MobBakery.Npc
{
    public class SimpleAnimateBakeryNpc : SimpleAnimate<BakeryNpcAnimType,BakeryNpcAnimation>
    {
        public void Start()
        {
            Animate();
        }
    }
}