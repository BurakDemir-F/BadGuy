using Generic.SO;
using UnityEngine;

namespace Generic.Items.SO
{
    [CreateAssetMenu(menuName = "Data/Config/BreakableItem", fileName = "CD_BreakableItem", order = 0)]
    public class CD_BreakableItem : ScriptableObject, IBreakableItemDataProvider
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _force;
        public AudioClip Clip => _clip;
        public float Force => _force;
    }

    public interface IBreakableItemDataProvider
    {
        AudioClip Clip { get; }
        float Force { get; }
    }
}