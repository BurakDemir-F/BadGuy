using System.Collections.Generic;
using System.Linq;
using Sequence.System;
using UnityEngine;
using UnityEngine.AI;

namespace Sequence.Input
{
    public class EnableNpc : SequenceNode
    {
        [SerializeField]private List<NavMeshAgent> _agents;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            foreach (var agent in _agents)
            {
                agent.enabled = true;
            }

            isNodeCompleted = true;
            SequenceNodeCompleted?.Invoke();
        }

#if UNITY_EDITOR
        [ContextMenu("Find Agents")]
        private void FindAgents()
        {
            _agents = FindObjectsOfType<NavMeshAgent>(true).ToList();
        }
#endif
    }
}