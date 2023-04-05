using System;
using System.Collections;
using System.Collections.Generic;
using Sequence.System;
using UnityEngine;

namespace LevelSpecific.MobBakery
{
    public class MobBakeryLevelStarter : MonoBehaviour
    {
        [SerializeField] private Sequencer _sequencer;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.1f);
            _sequencer.StartSequencer();
        }
    }
}