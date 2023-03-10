using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence.System
{
    public class SequenceNode : MonoBehaviour, ISequenceNode
    {
        [SerializeField] protected SequenceNodeType _nodeType;
        [SerializeField] private float _waitInterval;
        protected bool isNodeCompleted;
        
        // if we use same sequence node in sequencer we should add them to the queue so it can call them properly.
        public Queue<SequenceNode> NextSequenceQueue { get; private set; }
        protected SequenceNode CurrentNextSequence;
        public float WaitInterval => _waitInterval;
        public Action SequenceNodeCompleted { get; set; }
        public SequenceNodeType NodeType => _nodeType;

        public virtual void InitializeNode()
        {
            NextSequenceQueue ??= new Queue<SequenceNode>();
        }

        public virtual void StartSequenceNode()
        {
            CallNextNode();
        }

        protected virtual void CallNextNode()
        {
            if (NextSequenceQueue.Count == 0)
                return;

            CurrentNextSequence = NextSequenceQueue.Dequeue();
            
            switch (_nodeType)
            {
                case SequenceNodeType.None:
                    Debug.Log("sequence type none, this is not allowed.");
                    break;
                case SequenceNodeType.Sequential:
                    SequenceNodeCompleted += CallNextNodeOnComplete;
                    break;
                case SequenceNodeType.Parallel:
                    CurrentNextSequence.StartSequenceNode();
                    break;
                case SequenceNodeType.InTime:
                    StartCoroutine(StartNextNodeWithInterval());
                    break;
            }
        }

        protected virtual void CallNextNodeOnComplete()
        {
            SequenceNodeCompleted -= CallNextNodeOnComplete;
            CurrentNextSequence.StartSequenceNode();
        }

        private IEnumerator StartNextNodeWithInterval()
        {
            yield return new WaitForSeconds(WaitInterval);
            CurrentNextSequence.StartSequenceNode();
        }

        public bool IsCompleted() => isNodeCompleted;
    }

    public interface ISequenceNode
    {
        Queue<SequenceNode> NextSequenceQueue { get;}
        float WaitInterval { get; }
        Action SequenceNodeCompleted { get; set; }
        SequenceNodeType NodeType { get; }
        void InitializeNode();
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