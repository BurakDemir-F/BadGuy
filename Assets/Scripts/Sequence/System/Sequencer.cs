using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence.System
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField] private List<SequenceNode> _sequence;
        private int _currentNodeIndex;
        private SequenceNode _currentNode => _sequence[_currentNodeIndex == 0 ? 0 :_currentNodeIndex - 1];
        public event Action SequenceNodesCompleted;
        
        public void StartSequencer()
        {
            for (var i = 0; i < _sequence.Count; i++)
            {
                var node = _sequence[i];
                node.InitializeNode();
                node.SequenceNodeCompleted += OnNodeCompleted;
                
                if (i == _sequence.Count - 1) 
                    continue;

                var j = i + 1;
                while (j < _sequence.Count)
                {
                    var possibleNextSeq = _sequence[j];
                    if(possibleNextSeq.isActiveAndEnabled)
                    {
                        node.NextSequenceQueue.Enqueue(possibleNextSeq);
                        break;
                    }

                    j++;
                }
            }
            
            _sequence[0].StartSequenceNode();
        }

        private void OnDestroy()
        {
            foreach (var node in _sequence)
            {
                node.SequenceNodeCompleted -= OnNodeCompleted;
            }
        }

        private void OnNodeCompleted()
        {
            var isCompleted = _sequence.TrueForAll((node) => node.IsCompleted());
            if(isCompleted)
                SequenceNodesCompleted?.Invoke();
        }
    }
}