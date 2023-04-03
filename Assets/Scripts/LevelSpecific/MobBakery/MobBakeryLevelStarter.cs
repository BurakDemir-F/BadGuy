using System;
using Sequence.System;
using UnityEngine;

namespace LevelSpecific.MobBakery
{
    public class MobBakeryLevelStarter : MonoBehaviour
    {
        [SerializeField] private Sequencer _sequencer;

        private void Start()
        {
            _sequencer.StartSequencer();
        }
    }
}