using LevelSpecific.InformUI;
using Sequence.System;
using UnityEngine;

namespace Sequence
{
    public class InformUIOpen : SequenceNode
    {
        [SerializeField] private InformUI _informUI;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            _informUI.EnableInformUI();
            SequenceNodeCompleted?.Invoke();
        }
    }
}