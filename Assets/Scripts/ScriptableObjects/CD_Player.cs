using Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player", menuName = "Data/Config/Player", order = 0)]
    public class CD_Player : ScriptableObject,ISpeedProvider
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _gravity;

        public float MoveSpeed => _moveSpeed;
        public float RotateSpeed => _rotateSpeed;
        public float FallSpeed => _fallSpeed;
        public float Gravity => _gravity;
    }
}