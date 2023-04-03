using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence.MissionObjectives.SO
{
    [CreateAssetMenu(menuName = "Data/Config/MissionObjectives", fileName = "CD_MissionObjectives", order = 0)]
    public class CD_MissionObjectives : ScriptableObject
    {
        public Color NumberColor, ObjectiveColor;
        public float ShowDuration;
        public List<Objective> Objectives;

        private void OnValidate()
        {
            for (var i = 0; i < Objectives.Count; i++)
            {
                var objective = Objectives[i];
                objective.Number = i;
            }
        }
    }

    [Serializable]
    public class Objective
    {
        public int Number;
        public string ObjectiveDetail;
    }
}