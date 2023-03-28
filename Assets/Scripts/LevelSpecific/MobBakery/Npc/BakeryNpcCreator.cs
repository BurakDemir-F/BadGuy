using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace LevelSpecific.MobBakery.Npc
{
    public class BakeryNpcCreator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private AnimatorController _controller;
#endif
        [ContextMenu("Fill References")]
        private void FillReferences()
        {
            foreach (Transform child in transform)
            {
                var npcAnimator = child.GetComponent<BakeryNpcAnimator>();
                npcAnimator.SetAnimator(npcAnimator.GetComponent<Animator>());
                npcAnimator.SetController(_controller);

                var npc = child.GetComponent<BakeryNpc>();
                npc.SetNpcAnimator(npcAnimator);
                npc.SetAgent(child.GetComponent<NavMeshAgent>());
            }
        }
    }
}