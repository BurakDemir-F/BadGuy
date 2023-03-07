using Generic;
using Npc;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NPC", menuName = "Data/Config/NPC", order = 0)]
    public class CD_NPC : ScriptableObject,ISpeedProvider,INPCDataProvider
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _gravity;
        [SerializeField] private AttackType _attackType;
        [SerializeField] private LayerMask _attackLayer;
        [SerializeField] private float _chaseRangeRadius;
        [SerializeField] private float _attackRangeRadius;


        public float MoveSpeed => _moveSpeed;
        public float RotateSpeed => _rotateSpeed;
        public float FallSpeed => _fallSpeed;
        public float Gravity => _gravity;
        public AttackType AttackType => _attackType;
        public LayerMask AttackLayer => _attackLayer;
        public float ChaseRangeRadius => _chaseRangeRadius;
        public float AttackRangeRadius => _attackRangeRadius;
    }
}