using System.Collections;
using Generic.Animation;
using Sequence.System;
using UnityEngine;

namespace Sequence.Hi
{
    public class NpcHi : SequenceNode
    {
        [SerializeField] private SimpleAnimateNpc _npcAnim;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            _npcAnim.Animate();
        }

        private IEnumerator AnimateCor()
        {
            var wait = _npcAnim.Animate();
            yield return new WaitForSeconds(wait);
            isNodeCompleted = true;
            SequenceNodeCompleted?.Invoke();
        }
    }
}