using System;
using System.Collections.Generic;
using DG.Tweening;
using Sequence.System;
using Sequence.UIObjectsCamera;
using TMPro;
using UnityEngine;

namespace Sequence.Skip
{
    public class ShowOffSkipNode : SequenceNode
    {
        [SerializeField] private List<SequenceNode> _allNodes;
        [SerializeField] private List<SequenceNode> _afterSkipNodes;
        [SerializeField] private TextMeshProUGUI _skipText;
        [SerializeField] private SkipInputReceiver _inputReceiver;
        [SerializeField] private float _skipOptionDuration = 7;
        [SerializeField] private EnableUIObjectsCamera _enableUIObjectsCamera;
        [SerializeField] private DisableUIObjectsCameraNode _disableUIObjectsCamera;
        private float _counter;

        private bool _canReceiveInput;

        private void Start()
        {
            _skipText.gameObject.SetActive(false);
        }

        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            _inputReceiver.GameActionPerformed += OnInputReceived;
            _skipText.gameObject.SetActive(true);
            _canReceiveInput = true;
        }

        private void OnInputReceived(SkipInput obj)
        {
            if (obj == SkipInput.Skip && _canReceiveInput)
            {
                foreach (var node in _allNodes)
                {
                    node.StopNode();
                }
                
                _afterSkipNodes[0].StartSequenceNode();
                _skipText.gameObject.SetActive(false);
                _disableUIObjectsCamera.DisableCamera();
                DOVirtual.DelayedCall(2f, _enableUIObjectsCamera.EnableCamera);
            }
        }

        private void Update()
        {
            if (!_canReceiveInput)
                return;

            if (_counter >= _skipOptionDuration)
            {
                _skipText.gameObject.SetActive(false);
                _canReceiveInput = false;
                return;
            }

            _counter += Time.deltaTime;
        }
    }
}