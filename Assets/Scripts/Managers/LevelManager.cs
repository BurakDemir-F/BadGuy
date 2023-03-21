using System;
using System.Collections;
using InputRelated;
using Sequence.System;
using UnityEngine;

namespace Managers
{
    public class LevelManager : InputReceiver<LifeTimeInputTypes>
    {
        [SerializeField] private GameState _state;
        [SerializeField] private Sequencer _sequencer;
        [SerializeField] private InputManager _inputManager;

        public event Action<bool> LevelEnd;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            _state = GameState.ShowOff;
            _sequencer.SequenceNodesCompleted += OnSequenceCompleted;
            _sequencer.StartSequencer();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _sequencer.SequenceNodesCompleted -= OnSequenceCompleted;
        }
        
        protected override void OnGameActionPerformed(LifeTimeInputTypes gameActionType)
        {
            if (gameActionType == LifeTimeInputTypes.Escape)
            {
                if (_state == GameState.Playing)
                {
                    _state = GameState.Stopped;
                }
                else if(_state == GameState.Stopped)
                {
                    _state = GameState.Playing;
                }
            }
        }

        private void OnSequenceCompleted()
        {
            _state = GameState.Playing;
            _inputManager.ActivateInput(InputType.Movement);
            _inputManager.ActivateInput(InputType.PlayerInteraction);
        }
    }

    public enum LifeTimeInputTypes
    {
        None,
        Escape,
    }

    public enum GameState
    {
        None,
        ShowOff,
        Playing,
        Stopped
    }
}