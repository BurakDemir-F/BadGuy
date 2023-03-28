using System.Collections.Generic;
using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Npc.SO
{
    [CreateAssetMenu(menuName = "Data/Config/BakeryNpc", fileName = "CD_BakeryNpc", order = 0)]
    public class CD_BakeryNpc : ScriptableObject
    {
        public List<BakeryNpcData> NpcData;
        public BakeryNpcData TargetNpcData;
    }

    [System.Serializable]
    public class BakeryNpcData
    {
        public float Speed;
        public float WaitDuration;
        public IngredientSO WantedProduct;
    }
}