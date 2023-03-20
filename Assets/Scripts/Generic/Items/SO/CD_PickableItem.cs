using UnityEngine;

namespace Generic.Items.SO
{
    [CreateAssetMenu(menuName = "Data/Config/PickableItem", fileName = "CD_PickableItem", order = 0)]
    public class CD_PickableItem : ScriptableObject
    {
        public float PickTime;
    }
}