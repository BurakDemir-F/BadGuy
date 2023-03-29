using UnityEngine;

namespace LevelSpecific.MobBakery.Npc
{
    public class SitArea : MonoBehaviour
    {
        [SerializeField] private bool _isOccupied;
        public Transform SitTransform => transform;

        public bool IsOccupied
        {
            get => _isOccupied;
            set => _isOccupied = value;
        }
    }
}