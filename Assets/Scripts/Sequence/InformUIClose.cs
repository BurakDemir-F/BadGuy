using LevelSpecific.InformUI;
using Sequence.System;
using UnityEngine;

namespace Sequence
{
    public class InformUIClose : SequenceNode
    {
        [SerializeField] private InformUI _informUI;
        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            _informUI.DisableInformUI();
            SequenceNodeCompleted?.Invoke();
        }
    }
}