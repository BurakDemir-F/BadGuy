using System;
using System.Collections;
using UnityEngine;

namespace Sequence.System
{
    public class SequenceNode : MonoBehaviour, ISequenceNode
    {
        [SerializeField] private SequenceNodeType _nodeType;
        [SerializeField] private float _waitInterval;
        private bool _isCompleted;
        public SequenceNode NextSequence { get; set; }
        public float WaitInterval => _waitInterval;
        public Action SequenceNodeCompleted { get; set; }
        public SequenceNodeType NodeType => _nodeType;

        public virtual void StartSequenceNode()
        {
            CallNextNode();
        }

        protected virtual void CallNextNode()
        {
            if (NextSequence == null)
                return;

            switch (_nodeType)
            {
                case SequenceNodeType.None:
                    Debug.Log("sequence type none, this is not allowed.");
                    break;
                case SequenceNodeType.Sequential:
                    SequenceNodeCompleted += CallNextNodeOnComplete;
                    break;
                case SequenceNodeType.Parallel:
                    NextSequence.StartSequenceNode();
                    break;
                case SequenceNodeType.InTime:
                    StartCoroutine(StartNextNodeWithInterval());
                    break;
            }
        }

        protected virtual void CallNextNodeOnComplete()
        {
            SequenceNodeCompleted -= CallNextNodeOnComplete;
            NextSequence.StartSequenceNode();
        }

        private IEnumerator StartNextNodeWithInterval()
        {
            yield return new WaitForSeconds(NextSequence.WaitInterval);
            NextSequence.StartSequenceNode();
        }

        public bool IsCompleted() => _isCompleted;
    }

    public interface ISequenceNode
    {
        SequenceNode NextSequence { get; set; }
        float WaitInterval { get; }
        Action SequenceNodeCompleted { get; set; }
        SequenceNodeType NodeType { get; }
        void StartSequenceNode();
        bool IsCompleted();
    }

    public enum SequenceNodeType
    {
        None,
        Sequential,
        Parallel,
        InTime
    }
}