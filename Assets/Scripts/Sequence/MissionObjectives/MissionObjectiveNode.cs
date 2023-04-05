using System.Collections;
using System.Text;
using Sequence.MissionObjectives.SO;
using Sequence.System;
using TMPro;
using UnityEngine;
using Utilities;

namespace Sequence.MissionObjectives
{
    public class MissionObjectiveNode : SequenceNode
    {
        [SerializeField] private TextMeshProUGUI _missionsText;
        [SerializeField] private GameObject _textRoot;
        [SerializeField] private CD_MissionObjectives _objectives;

        private Coroutine _nodeCor;
        
        public override void InitializeNode()
        {
            base.InitializeNode();
            ChangeTextActivation(false);
        }

        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            ChangeTextActivation(true);
            var sb = new StringBuilder();
            var numColor = _objectives.NumberColor;
            var textColor = _objectives.ObjectiveColor;

            foreach (var mission in _objectives.Objectives)
            {
                sb.Append(
                    $"{mission.Number.ToString().GetColored(numColor)} - {mission.ObjectiveDetail.GetColored(textColor)}\n");
            }

            _missionsText.text = sb.ToString();
            _nodeCor = StartCoroutine(ShowMissionObjectivesCor());
        }

        private IEnumerator ShowMissionObjectivesCor()
        {
            yield return new WaitForSeconds(_objectives.ShowDuration);
            ChangeTextActivation(false);
        }

        public override void StopNode()
        {
            base.StopNode();
            if(_nodeCor != null)
                StopCoroutine(_nodeCor);
            ChangeTextActivation(false);
        }

        private void ChangeTextActivation(bool status)
        {
            _textRoot.gameObject.SetActive(status);
            _missionsText.gameObject.SetActive(status);
        }
    }
}