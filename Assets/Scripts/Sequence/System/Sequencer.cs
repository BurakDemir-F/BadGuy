using System.Collections.Generic;
using UnityEngine;

namespace Sequence.System
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField] private List<SequenceNode> _sequence;
        private int _currentNodeIndex;
        private SequenceNode _currentNode => _sequence[_currentNodeIndex == 0 ? 0 :_currentNodeIndex - 1];
        
        private void Start()
        {
            for (var i = 0; i < _sequence.Count; i++)
            {
                var node = _sequence[i];
                if (i == _sequence.Count - 1) 
                    continue;
                
                node.NextSequence = _sequence[i + 1];
            }

            _sequence[0].StartSequenceNode();
        }

        private void OnNodeCompleted()
        {
            var isCompleted = _sequence.TrueForAll((node) => node.IsCompleted());
        }
    }
}