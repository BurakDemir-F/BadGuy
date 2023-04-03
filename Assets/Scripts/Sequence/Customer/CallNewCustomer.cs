using LevelSpecific.MobBakery.Machines.CustomerArea;
using Sequence.System;
using UnityEngine;

namespace Sequence.Customer
{
    public class CallNewCustomer : SequenceNode
    {
        [SerializeField] private CustomerArea _customerArea;

        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            _customerArea.CallNewCustomer();
            SequenceNodeCompleted?.Invoke();
        }
    }
}